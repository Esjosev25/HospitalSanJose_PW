using AutoMapper;
using HospitalSanJoseAPI.Models;
using HospitalSanJoseModel;
using HospitalSanJoseModel.DTO.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly HospitalDbContext _context;
        public AuthController(HospitalDbContext context, ILogger<AuthController> logger, IMapper mapper, IConfiguration configuration)
        {

            _context = context;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }
        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<JWTResponse>> Login(Login login)
        {
            var response = new Response();
            var JWTResponse = new JWTResponse();


            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);
            // Look up the user in the database
            if (user == null || user.Deleted)
            {
            response.AlertIcon = "error";
            response.AlertMessage = "Error en iniciar sesion";
                JWTResponse.Response = response;

                return BadRequest(JWTResponse);
            }

            if (user.IsLocked)
            {
                response.AlertIcon = "warning";
                response.AlertMessage = "Usuario bloqueado";
                JWTResponse.Response = response;
                return BadRequest(JWTResponse);
            }

            if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {


                _logger.LogInformation($"El usuario {user.Username} inició sesión");


                //Get user Roles
                var userRoles = (from ur in _context.UserRoles
                                 join r in _context.Roles on ur.RoleId equals r.Id
                                 orderby r.Name ascending
                                 where ur.UserId == user.Id
                                 select r.Name).ToList();
                var roles = string.Join(",", userRoles);


                JWTResponse.UserId = user.Id;
                JWTResponse.UserName = user.Username;
                JWTResponse.Roles = roles;




                var Token = CustomTokenJWT(JWTResponse);
                JWTResponse.Token = Token;
                return Ok(JWTResponse);
            }

            response.AlertIcon = "error";
            response.AlertMessage = "Error en iniciar sesion";
            JWTResponse.Response = response;
            return BadRequest(JWTResponse);

        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {

            if (_context.Users == null)
            {
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");
            }
            var userDB = _context.Users.FirstOrDefault(u => u.Username == register.Username || u.Email == register.Email);
            var response = new HospitalSanJoseModel.Response();
            if (userDB != null)
            {

                response.AlertMessage = "Correo o Usuario ya existen, intenta con uno nuevo";
                response.AlertIcon = "error";
                register.Response = response;

                return BadRequest(register);
            }
            if (register.Password != register.ConfirmPassword)
            {
                response.AlertMessage = "Asegúrate de que tus contraseñas coincidan";
                response.AlertIcon = "error";
                register.Response = response;
                return BadRequest(register);
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            register.Password = BCrypt.Net.BCrypt.HashPassword(register.Password, salt);

            var newUser = _mapper.Map<Models.User>(register);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            // Asignar rol de usuario
            var pacienteRole = await _context.Roles.Where(r => r.Name == Utils.Roles.RolesType.Paciente.ToString()).FirstOrDefaultAsync();
            if (pacienteRole != null)
            {

                var newRole = new Models.UserRole()
                {
                    RoleId = pacienteRole.Id,
                    UserId = newUser.Id,
                };
                _context.UserRoles.Add(newRole);
                await _context.SaveChangesAsync();
            }
            register.UserId = newUser.Id;
            _logger.LogInformation($"Se registró el usuario {register.Username} con el correo {register.Email}");
            return Ok(register);


        }


        private string CustomTokenJWT(JWTResponse Payload)
        {
            var _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
            var _signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var _Header = new JwtHeader(_signingCredentials);
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, Payload.UserName),
                new Claim("UserId", Payload.UserId.ToString()),
                new Claim("Username", Payload.UserName),
                new Claim("Roles", Payload.Roles)
            };
            var _Payload = new JwtPayload(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: _Claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.AddDays(1)
                );
            var _Token = new JwtSecurityToken(_Header, _Payload);
            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}

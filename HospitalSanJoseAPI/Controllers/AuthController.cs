using AutoMapper;
using HospitalSanJoseAPI.Models;
using HospitalSanJoseModel.DTO.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;
        private readonly HospitalDbContext _context;
        public AuthController(HospitalDbContext context, ILogger<AuthController> logger, IMapper mapper)
        {

            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(Login login)
        {
            var response = new HospitalSanJoseModel.Response();
            login.Response = response;

            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username);
            // Look up the user in the database
            login.Response.AlertIcon = "error";
            login.Response.AlertMessage = "Error en iniciar sesion";
            if (user == null || user.Deleted)
            {


                return BadRequest(login);
            }

            if (user.IsLocked)
            {
                login.Response.AlertIcon = "warning";
                login.Response.AlertMessage = "Usuario bloqueado";
                return BadRequest(login);
            }

            if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {

                //login.Response.ShowWarning = false;
                login.Response = null;
                _logger.LogInformation($"El usuario {user.Username} inició sesión");


                login.UserId = user.Id;
                return Ok(login);
            }


            return BadRequest(login);

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

            var newUser = _mapper.Map<User>(register);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            // Asignar rol de usuario
            var pacienteRole = await _context.Roles.Where(r => r.Name == Utils.Roles.RolesType.Paciente.ToString()).FirstOrDefaultAsync();
            if (pacienteRole != null)
            {

                var newRole = new UserRole()
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
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using DTO = HospitalSanJoseModel.DTO;
using AutoMapper;
using System.Linq;
using HospitalSanJoseModel.DTO.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly HospitalDbContext _context;
        public UsersController(HospitalDbContext context, IMapper mapper, ILogger<UsersController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var users = _mapper.Map<IEnumerable<HospitalSanJoseModel.User>>(await (from u in _context.Users
                                                                                   where !u.Deleted
                                                                                   select u).ToListAsync());
            return Ok(users);
        }

        // GET: api/Users/InactiveUsers
        [HttpGet("InactiveUsers")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.User>>> GetInactiveUsers(int id =0)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
             var usersList = new List<HospitalSanJoseModel.User>();
            if (id != 0)
            {
                var user =  _mapper.Map<HospitalSanJoseModel.User>(await _context.Users.FindAsync(id));
                if(user!=null)
                usersList.Add(user);
            }
             
            var users = _mapper.Map<IEnumerable<HospitalSanJoseModel.User>>(await (from u in _context.Users
                                                                                   where !u.Activated && !u.Deleted && u.Id != id
                                                                                   select u).ToListAsync());
            usersList.AddRange(users);
            return Ok(usersList);
        }

        // GET: api/Users/UsersWithRemainingRoles
        [HttpGet("UsersWithRemainingRoles")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.User>>> UsersWithRemainingRoles()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var users = _mapper.Map<IEnumerable<HospitalSanJoseModel.User>>(await (from user in _context.Users
                                                                                   join userRole in _context.UserRoles
                                                                                   on user.Id equals userRole.UserId into userRoles
                                                                                   from role in userRoles.DefaultIfEmpty()
                                                                                   group role by user into userGroup
                                                                                   where userGroup.Count() < 4 && userGroup.Key.Deleted == false
                                                                                   select userGroup.Key).ToListAsync());
            return Ok(users);
        }

        // GET: api/Users/NonDoctors
        [HttpGet("NonDoctors")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.User>>> AllNonDoctorUsers()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var doctorsIds = _context.Doctors.Select(d => d.UserId).ToList();
            var nonDoctorUsers = _mapper.Map<IEnumerable<HospitalSanJoseModel.User>>(await _context.Users.Where(u => !doctorsIds.Contains(u.Id) && !u.Deleted).ToListAsync());

            return Ok(nonDoctorUsers);
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = _mapper.Map<HospitalSanJoseModel.User>(await _context.Users.FindAsync(id));

            if (user != null)
            {
            return Ok(user);
            }

                return NotFound();
        }
        // GET: api/Users/5
        [HttpGet("ByDoctorId/{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.User>> GetUserByDoctorId(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FindAsync(id);
            var user = _mapper.Map<HospitalSanJoseModel.User>(await _context.Users.FindAsync(doctor.UserId));

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound();
        }


        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, HospitalSanJoseModel.User user)
        {
            if (id != user.Id)
            {

                return BadRequest(new
                {
                    message = "Parametro por ruta y el id de usuario deben de ser igual",
                    error = true
                });
            }
            var userDB = _context.Users.FirstOrDefault(u => u.Id != user.Id && (u.Username == user.Username || u.Email == user.Email ));

            if (userDB != null)
            {
                var response = new HospitalSanJoseModel.Response();
                response.AlertMessage = "Correo o Usuario ya existen, intenta con uno nuevo";
                response.AlertIcon = "error";
                user.Response = response;
                return BadRequest(user);
            }
            var userPassword = _context.Users.AsNoTracking().First(u => u.Id == id);
            if (user.Password != userPassword.Password)
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

            }
            var changedUser= _mapper.Map<User>(user);
            _context.Entry(changedUser).State = EntityState.Modified;

            try
            {

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Se actualizó el usuario Id: {id} username: {user.Username} email: {user.Email}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // PUT: api/Users/ChangePassword/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ChangePassword/{id}")]
        public async Task<IActionResult> ChangePassword(int id, ProfileChangePassword changePassword)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                 var response = new HospitalSanJoseModel.Response();
                changePassword.User = _mapper.Map<HospitalSanJoseModel.User>(user);
                if (!BCrypt.Net.BCrypt.Verify( changePassword.OldPassword, user.Password)){
                    response.AlertMessage = "Contrasena incorrecta";
                    response.AlertIcon = "error";
                    changePassword.Response = response;
                    return BadRequest(changePassword);
                }
                if (changePassword.NewPassword != changePassword.ConfirmPassword)
                {
                    //Psw deben de ser iguales
                    response.AlertMessage = "Las contrasenas deben de coincidir, intenta de nuevo";
                    response.AlertIcon = "warning";
                    changePassword.Response = response;
                    return BadRequest(changePassword);
                }
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword, salt);
                
                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Se bloqueó el usuario Id: {id} username: {user.Username} email: {user.Email}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PATCH: api/Users/Block/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("Block/{id}")]
        public async Task<IActionResult> ToggleBlockUser(int id)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                user.IsLocked = !user.IsLocked;
                _context.Update(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Se bloqueó el usuario Id: {id} username: {user.Username} email: {user.Email}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HospitalSanJoseModel.User>> PostUser(DTO.User.UserCreate user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");
            }
            var userDB = _context.Users.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
            var response = new HospitalSanJoseModel.Response();
            user.Response = response;
            if (userDB != null)
            {
                
                response.AlertMessage = "Correo o Usuario ya existen, intenta con uno nuevo";
                response.AlertIcon = "error";
                
                return BadRequest(user);
            }
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

            var newUser = _mapper.Map<User>(user);
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
            _logger.LogInformation($"Se registró el usuario {user.Username} con el correo {user.Email}");
            return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
        }

        //// DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Deleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Se eliminó el usuario Id: {id} username: {user.Username} email: {user.Email}");
            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

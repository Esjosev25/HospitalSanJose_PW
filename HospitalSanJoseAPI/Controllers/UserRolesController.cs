using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using DTO = HospitalSanJoseModel.DTO.UserRoles;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserRolesController : ControllerBase
    {
        private readonly HospitalDbContext _context;
        private readonly IMapper _mapper;
        public UserRolesController(HospitalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/UserRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.UserRole>>> GetUserRoles()
        {
            if (_context.UserRoles == null)
            {
                return NotFound();
            }
            var userRoles = _mapper.Map<IEnumerable<HospitalSanJoseModel.UserRole>>(await _context.UserRoles
                                                                                                            .Include(u => u.User)
                                                                                                            .Include(r => r.Role)
                                                                                                            .Where(ur => !ur.User.Deleted)
                                                                                                            .OrderBy(ur => ur.UserId)
                                                                                                            .ToListAsync());
            return Ok(userRoles);
        }

        // GET: api/UserRoles/ByUser
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("Session/ByUser/{userId}")]
        [HttpGet]
        public ActionResult<DTO.UserRolesSession> GetUserRoleById(int userId)
        {
            if (_context.UserRoles == null)
            {
                return Problem("Entity set 'HospitalDbContext.UserRoles'  is null.");
            }
            var userRoles = (from ur in _context.UserRoles
                             join r in _context.Roles on ur.RoleId equals r.Id
                             orderby r.Name ascending
                             where ur.UserId == userId
                             select r.Name).ToList();
            var roles = string.Join(",", userRoles);

            var userRolesSession = new DTO.UserRolesSession()
            {
                UserId = userId,
                Roles = roles
            };

            return Ok(userRolesSession);
        }
        [Route("ByUser/{userId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.UserRole>>> GetUserRolesById(int userId)
        {
            if (_context.UserRoles == null)
            {
                return Problem("Entity set 'HospitalDbContext.UserRoles'  is null.");
            }
            var userRoles = _mapper.Map<IEnumerable<HospitalSanJoseModel.UserRole>>(await _context.UserRoles
                                                                                                            .Include(u => u.User)
                                                                                                            .Include(r => r.Role)
                                                                                                            .Where(ur => ur.User.Id == userId)
                                                                                                            .OrderBy(ur => ur.UserId)
                                                                                                            .ToListAsync());


            return Ok(userRoles);
        }

        // POST: api/UserRole
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DTO.UserRolesCreate>> PostUserRole(HospitalSanJoseModel.UserRole userRole)
        {
            if (_context.UserRoles == null)
            {
                return Problem("Entity set 'HospitalDbContext.Departments'  is null.");
            }
            var userRoleDB = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.RoleId == userRole.RoleId && ur.UserId == userRole.UserId);
            var response = new HospitalSanJoseModel.Response();
            if (userRoleDB != null)
            {
                response.AlertMessage = "El usuario ya tiene asignado dicho rol";
                response.AlertIcon = "Error";
                userRole.Response = response;
                return BadRequest(userRole);
            }

            var newuserRole = _mapper.Map<UserRole>(userRole);
            _context.UserRoles.Add(newuserRole);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<DTO.UserRolesCreate>(newuserRole));
        }

        // DELETE: api/UserRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            if (_context.UserRoles == null)
            {
                return NotFound();
            }
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null)
            {
                return NotFound();
            }

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}

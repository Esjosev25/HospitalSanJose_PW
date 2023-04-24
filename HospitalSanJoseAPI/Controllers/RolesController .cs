using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RolesController> _logger;
        private readonly HospitalDbContext _context;


        public RolesController(HospitalDbContext context, IMapper mapper, ILogger<RolesController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Role>>> GetRoles()
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var personalInfos = _mapper.Map<IEnumerable<HospitalSanJoseModel.Role>>(await _context.Roles.ToListAsync());
            return Ok(personalInfos);
        }

        // GET: api/Roles/AvailableRolesForUser
        [HttpGet("AvailableRolesForUser/{userId}")]

        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Role>>> AvailableRolesForUser(int? userId)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var userRoles = await (from ur in _context.UserRoles
                                   join r in _context.Roles on ur.RoleId equals r.Id
                                   orderby r.Name ascending
                                   where ur.UserId == userId
                                   select r.Id).ToListAsync();
            var userRolesAvailable = _mapper.Map<IEnumerable<HospitalSanJoseModel.Role>>(await _context.Roles.Where(r => !userRoles.Contains(r.Id)).ToListAsync());
            return Ok(userRolesAvailable);
        }



        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.Role>> GetRole(int id)
        {
            if (_context.Roles == null)
            {
                return NotFound();
            }
            var role = _mapper.Map<HospitalSanJoseModel.Role>(await _context.Roles.FirstOrDefaultAsync(r => r.Id == id));

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

    }
}

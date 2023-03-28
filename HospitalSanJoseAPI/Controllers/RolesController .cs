using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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



        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.Role>> GetRole(int id)
        {
          if (_context.Roles == null)
          {
              return NotFound();
          }
            var role = _mapper.Map<HospitalSanJoseModel.Role>(await _context.Roles.FirstOrDefaultAsync(r=>r.Id==id));

            if (role == null)
            {
                return NotFound();
            }

            return Ok(role);
        }

        // GET: api/Roless/User/5
        //[HttpGet("User/{id}")]
        //public async Task<ActionResult<HospitalSanJoseModel.Role>> GetRolesByUser(int id)
        //{
        //    if (_context.Roles == null)
        //    {
        //        return NotFound();
        //    }
        //    var personalInfo = _mapper.Map<HospitalSanJoseModel.Role>(await _context.Roles.Include(p => p.User).FirstOrDefaultAsync(pi => pi.UserId == id));

        //    if (personalInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(personalInfo);
        //}

      
    }
}

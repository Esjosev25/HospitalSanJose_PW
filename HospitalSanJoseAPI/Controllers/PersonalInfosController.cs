using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using DTO = HospitalSanJoseModel.DTO.PersonalInfo;
using AutoMapper;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalInfosController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PersonalInfosController> _logger;
        private readonly HospitalDbContext _context;


        public PersonalInfosController(HospitalDbContext context, IMapper mapper, ILogger<PersonalInfosController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/PersonalInfos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.PersonalInfo>>> GetPersonalInfos()
        {
            if (_context.PersonalInfos == null)
            {
                return NotFound();
            }
            var personalInfos = _mapper.Map<IEnumerable<HospitalSanJoseModel.PersonalInfo>>(await _context.PersonalInfos.Include(p => p.User).Where(p => !p.User.Deleted).ToListAsync());
            return Ok(personalInfos);
        }



        // GET: api/PersonalInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.PersonalInfo>> GetPersonalInfo(int id)
        {
            if (_context.PersonalInfos == null)
            {
                return NotFound();
            }
            var personalInfo = _mapper.Map<HospitalSanJoseModel.PersonalInfo>(await _context.PersonalInfos.Include(p => p.User).FirstOrDefaultAsync(pi => pi.Id == id));

            if (personalInfo == null)
            {
                return NotFound();
            }

            return Ok(personalInfo);
        }

        // GET: api/PersonalInfos/User/5
        [HttpGet("User/{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.PersonalInfo>> GetPersonalInfoByUser(int id)
        {
            if (_context.PersonalInfos == null)
            {
                return NotFound();
            }
            var personalInfo = _mapper.Map<HospitalSanJoseModel.PersonalInfo>(await _context.PersonalInfos.Include(p => p.User).FirstOrDefaultAsync(pi => pi.UserId == id));

            if (personalInfo == null)
            {
                return NotFound();
            }

            return Ok(personalInfo);
        }

        // PUT: api/PersonalInfos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonalInfo(int id, DTO.PersonalInfoUpdate personalInfo)
        {
            var response = new HospitalSanJoseModel.Response();
            if (id != personalInfo.Id)
            {
                response.AlertMessage = "Parametro por ruta y el id de personalInfo deben de ser igual";
                response.AlertIcon = "error";
                personalInfo.Response = response;
                return BadRequest(personalInfo);
            }

            var personalInfoDB = _context.PersonalInfos.Where(pi=>pi.UserId == personalInfo.UserId && pi.Id != id).FirstOrDefault();
            if(personalInfoDB != null)
            {
                response.AlertMessage = "El usuario destino ya posee información asociada";
                response.AlertIcon = "error";
                personalInfo.Response = response;
                return BadRequest(personalInfo);
            }
           
            var newPersonalInfo = _mapper.Map<PersonalInfo>(personalInfo);            
            _context.Entry(newPersonalInfo).State = EntityState.Modified;

            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalInfoExists(id))
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

        // POST: api/PersonalInfos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HospitalSanJoseModel.PersonalInfo>> PostPersonalInfo(DTO.PersonalInfoCreate personalInfo)
        {
          if (_context.PersonalInfos == null)
          {
              return Problem("Entity set 'HospitalDbContext.PersonalInfos'  is null.");
          }
            
            var personalInfoDb = await _context.PersonalInfos.FirstOrDefaultAsync(pi => pi.UserId == personalInfo.UserId);
            var response = new HospitalSanJoseModel.Response();
            personalInfo.Response = response;
            if (personalInfoDb != null)
            {

                response.AlertMessage = "El usuario ya posee información asociada";
                response.AlertIcon = "error";

                return BadRequest(personalInfo);
            }

            var newPersonalInfo = _mapper.Map<PersonalInfo>(personalInfo);
            _context.PersonalInfos.Add(newPersonalInfo);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FindAsync(personalInfo.UserId);
            user.Activated = true;
            _context.Update(user);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetPersonalInfo", new { id = newPersonalInfo.Id }, newPersonalInfo);
        }

        // DELETE: api/PersonalInfos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonalInfo(int id)
        {
            if (_context.PersonalInfos == null)
            {
                return NotFound();
            }
            var personalInfo = await _context.PersonalInfos.FindAsync(id);
            if (personalInfo == null)
            {
                return NotFound();
            }
            _context.PersonalInfos.Remove(personalInfo);
            var user= await _context.Users.FindAsync(personalInfo.UserId);
            user.Activated = false;
            _context.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonalInfoExists(int id)
        {
            return (_context.PersonalInfos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

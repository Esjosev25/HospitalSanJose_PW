using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DTO = HospitalSanJoseModel.DTO.DoctorDepartment;
using HospitalSanJoseAPI.Models;
using AutoMapper;


namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DoctorDepartmentsController : ControllerBase
    {
        private readonly HospitalDbContext _context;
        private readonly IMapper _mapper;

        public DoctorDepartmentsController(HospitalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/DoctorDepartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.DoctorDepartment>>> GetDoctorDepartments()
        {
            if (_context.DoctorDepartments == null)
            {
                return NotFound();
            }
            var doctorDepartments = _mapper.Map<IEnumerable<HospitalSanJoseModel.DoctorDepartment>>(await _context.DoctorDepartments.Include(d => d.Doctor).Include(u=>u.Doctor.User).Include(d => d.Department).ToListAsync());
            return Ok(doctorDepartments);
        }

        // GET: api/ByDoctor/5
        [Route("ByDoctorId/{doctorId}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.DoctorDepartment>>> GetDoctorDepartment(int doctorId)
        {
            if (_context.DoctorDepartments == null)
            {
                return NotFound();
            }
            
            var doctorDepartment = _mapper.Map<IEnumerable<HospitalSanJoseModel.DoctorDepartment>>(await _context.DoctorDepartments.Include(d => d.Doctor).Include(d => d.Department).Include(u=>u.Doctor.User).Where(dd=>dd.DoctorId == doctorId).ToListAsync());

            if (doctorDepartment == null)
            {
                return NotFound();
            }

            return Ok(doctorDepartment);
        }



        // POST: api/DoctorDepartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DTO.DoctorDepartmentCreate>> PostDoctorDepartment(DTO.DoctorDepartmentCreate doctorDepartment)
        {
            if (_context.DoctorDepartments == null)
            {
                return Problem("Entity set 'HospitalDbContext.DoctorDepartments'  is null.");
            }
            var doctorDepartmentDb = await _context.DoctorDepartments.Where(d => d.DoctorId == doctorDepartment.DoctorId && d.DepartmentId == doctorDepartment.DepartmentId).FirstOrDefaultAsync() ;
            var response = new HospitalSanJoseModel.Response();
            if (doctorDepartmentDb != null)
            {
                response.AlertMessage = "El usuario ya tiene asignado dicho rol";
                response.AlertIcon = "Error";
                doctorDepartment.Response = response;
                return BadRequest(doctorDepartment);
            }
            var newDoctorDepartment = _mapper.Map<DoctorDepartment>(doctorDepartment);
            _context.DoctorDepartments.Add(newDoctorDepartment);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<DTO.DoctorDepartmentCreate>(newDoctorDepartment));
        }


        // DELETE: api/DoctorDepartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctorDepartment(int id)
        {
            if (_context.DoctorDepartments == null)
            {
                return NotFound();
            }
            var doctorDepartment = await _context.DoctorDepartments.FindAsync(id);
            if (doctorDepartment == null)
            {
                return NotFound();
            }

            _context.DoctorDepartments.Remove(doctorDepartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

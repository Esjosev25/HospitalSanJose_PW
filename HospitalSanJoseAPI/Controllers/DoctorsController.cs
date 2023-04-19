using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;

using DTO = HospitalSanJoseModel.DTO.Doctor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DoctorsController : ControllerBase
    {
        private readonly HospitalDbContext _context;
        private readonly IMapper _mapper;

        public DoctorsController(HospitalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Doctor>>> GetDoctors()
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctors = _mapper.Map<IEnumerable<HospitalSanJoseModel.Doctor>>(await _context.Doctors.Include(u => u.User).ToListAsync());
            return Ok(doctors);
        }

        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.Doctor>> GetDoctor(int id)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctor = _mapper.Map<HospitalSanJoseModel.Doctor>(await _context.Doctors.FindAsync(id));

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        //// PUT: api/Doctors/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDoctor(int id, HospitalSanJoseModel.Doctor doctor)
        //{
        //    if (id != doctor.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(doctor).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DoctorExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Doctors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HospitalSanJoseModel.Doctor>> PostDoctor(DTO.DoctorCreate doctor)
        {
            var response = new HospitalSanJoseModel.Response();
            try
            {
                if (_context.Doctors == null)
                {
                    return Problem("Entity set 'HospitalDbContext.Doctors'  is null.");
                }
                var doctorDB = await _context.Doctors.Where(d => d.UserId == doctor.UserId).FirstOrDefaultAsync();
                if (doctorDB != null)
                {
                    response.AlertIcon = "error";
                    response.AlertMessage = "El usuario ya está registrado como doctor";
                    doctor.Response = response;
                }
                var newDoctor = _mapper.Map<Doctor>(doctor);
                _context.Doctors.Add(newDoctor);
                await _context.SaveChangesAsync();
                // Asignar rol de usuario
                var doctorRole = await _context.Roles.Where(r => r.Name == Utils.Roles.RolesType.Doctor.ToString()).FirstOrDefaultAsync();
                if (doctorRole != null)
                {
                    var isAlreadyAssigned = await _context.UserRoles.Where(ur => ur.UserId == doctor.UserId && ur.RoleId == doctorRole.Id).FirstOrDefaultAsync();
                    if (isAlreadyAssigned == null)
                    {
                        var newRole = new UserRole()
                        {
                            RoleId = doctorRole.Id,
                            UserId = doctor.UserId,
                        };
                        _context.UserRoles.Add(newRole);
                        await _context.SaveChangesAsync();
                    }

                }
                return CreatedAtAction("GetDoctor", new { id = newDoctor.Id }, doctor);
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }

        }

        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool DoctorExists(int id)
        //{
        //    return (_context.Doctors?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

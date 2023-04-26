using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;
using DTO = HospitalSanJoseModel.DTO.Doctor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;


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

        // GET: api/Doctors
        [HttpGet("DoctorsWithRemainingDepartments")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Doctor>>> GetDoctorsWithRemainingDepartments()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var countDepartments = await _context.Departments.CountAsync();

            var doctorsId = (await (from doctor in _context.Doctors
                                    join user in _context.Users
                                    on doctor.UserId equals user.Id
                                    join DoctorDepartment in _context.DoctorDepartments
                                    on doctor.Id equals DoctorDepartment.DoctorId into DoctorDepartments
                                    from department in DoctorDepartments.DefaultIfEmpty()
                                    group department by doctor into doctorGroup
                                    where doctorGroup.Count() < countDepartments
                                    select doctorGroup.Key.UserId).ToListAsync());
            var doctors = _mapper.Map<IEnumerable<HospitalSanJoseModel.Doctor>>(await _context.Doctors.Include(u => u.User).Where(d => doctorsId.Contains(d.UserId) && !d.User.Deleted).ToListAsync());
            return Ok(doctors);
        }
        // GET: api/Doctors
        [HttpGet("DoctorsByDepartment/{departmentId}")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Doctor>>> GeDoctorsByDepartment(int departmentId)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctors = _mapper.Map<IEnumerable<HospitalSanJoseModel.Doctor>>(await _context.DoctorDepartments.Where(dd=> dd.DepartmentId == departmentId).Include(d=>d.Doctor).ThenInclude(u=>u.User).Select(dd=>dd.Doctor).ToListAsync());
            
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
            var doctor = _mapper.Map<HospitalSanJoseModel.Doctor>(await _context.Doctors.Include(u => u.User).Where(u=>u.Id == id).FirstAsync());

            if (doctor == null)
            {
                return NotFound();
            }

            return Ok(doctor);
        }

        // PUT: api/Doctors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDoctor(int id, HospitalSanJoseModel.Doctor doctor)
        {
            var response = new HospitalSanJoseModel.Response();
            if (id != doctor.Id)
            {
                response.AlertMessage = "Parametro por ruta y el id de personalInfo deben de ser igual";
                response.AlertIcon = "error";
                return BadRequest(doctor);
            }


            var updatedDoctor = _mapper.Map<Doctor>(doctor);
            _context.Entry(updatedDoctor).State = EntityState.Modified;
            


            await _context.SaveChangesAsync();


            return NoContent();
        }

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

    }
}

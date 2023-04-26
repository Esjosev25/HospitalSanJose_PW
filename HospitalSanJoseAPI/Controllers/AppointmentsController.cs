using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;
using DTO = HospitalSanJoseModel.DTO.Appointment;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentsController : ControllerBase
    {
        private readonly HospitalDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentsController(HospitalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Appointments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Appointment>>> GetAppointments()
        {
          if (_context.Appointments == null)
          {
              return NotFound();
          }
            
            var appointments = _mapper.Map<IEnumerable<HospitalSanJoseModel.Appointment>>(await _context.Appointments.Include(u => u.User).Include(d => d.Doctor.User).OrderBy(a=>a.AppointmentDate ).ThenBy(a=>a.AppointmentTime).ToListAsync());
            return Ok(appointments);
        }

     

        // GET: api/Appointments/ByDoctor/5
        [HttpGet("ByDoctor/{doctorId}")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Appointment>>> GetAppointmentByDoctor(int doctorId)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointment = _mapper.Map<IEnumerable<HospitalSanJoseModel.Appointment>>(await _context.Appointments.Include(u => u.User).Include(d => d.Doctor.User).OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).Where(a=>a.DoctorId==doctorId).ToListAsync());

            return Ok(appointment);
        }

        // GET: api/Appointments/ByPacient/5
        [HttpGet("ByPacient/{userId}")]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Appointment>>> GetAppointmentByByPacient(int userId)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointments = _mapper.Map<IEnumerable<HospitalSanJoseModel.Appointment>>(await _context.Appointments.Include(u => u.User).Include(d => d.Doctor.User).OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime).Where(a => a.UserId == userId).ToListAsync());
            return Ok(appointments);
        }

        // GET: api/Appointments/AttentionHours
        [HttpGet("AttentionHours/{date}/{doctorId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAttentionHours(DateTime date, int doctorId)
        {
            var attentionHours = Utils.AttentionHours.GetAttentionHours();
            var attentionHoursTaken = await _context.Appointments.Where(a => a.AppointmentDate == date && a.DoctorId==doctorId).Select(a=>a.AppointmentTime.ToString()).ToListAsync();
            var freeHours = attentionHours.Except(attentionHoursTaken).ToList();
            return Ok(freeHours);
        }

        // GET: api/Appointments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.Appointment>> GetAppointment(int id)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointment = _mapper.Map<HospitalSanJoseModel.Appointment>(await _context.Appointments.Include(u => u.User).Include(d => d.Doctor.User).Where(a=>a.Id==id).FirstOrDefaultAsync());

            if (appointment == null)
            {
                return NotFound();
            }

            return appointment;
        }

        // POST: api/Appointments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DTO.AppointmentCreate>> PostAppointment(DTO.AppointmentCreate appointment)
        {
            if (_context.Appointments == null)
            {
                return Problem("Entity set 'HospitalDbContext.Appointments'  is null.");
            }
            var response = new HospitalSanJoseModel.Response();
            var doctorDb = await _context.Doctors.Include(d=>d.User).Where(d=>d.Id == appointment.DoctorId).FirstOrDefaultAsync();
            if(appointment.UserId == doctorDb.UserId)
            {
                response.AlertMessage = "Doctor y Paciente deben de ser distintos";
                response.AlertIcon = "error";
                appointment.Response = response;
                return BadRequest(appointment);
            }
            
            var doctorAppointmentDb = await _context.Appointments.FirstOrDefaultAsync(a => a.DoctorId==appointment.DoctorId  && a.AppointmentDate == appointment.AppointmentDate && a.AppointmentTime == TimeSpan.Parse(appointment.AppointmentTime!));
            if (doctorAppointmentDb != null)
            {
                var doctor = await _context.Doctors.Include(d=>d.User).Where(d=>d.UserId==appointment.DoctorId).FirstOrDefaultAsync();
                response.AlertMessage = $"El doctor {doctor.User.FirstName} {doctor.User.LastName} tiene una cita agendada a esa hora, intenta en otro horario ";
                response.AlertIcon = "error";
                appointment.Response = response;
                return BadRequest(appointment);
            }
            var userAppointmentDb = await _context.Appointments.FirstOrDefaultAsync(a => a.UserId == appointment.UserId && a.AppointmentDate == appointment.AppointmentDate && a.AppointmentTime == TimeSpan.Parse(appointment.AppointmentTime!));
            if (userAppointmentDb != null)
            {
                var user =await _context.Users.FindAsync(appointment.UserId);
                response.AlertMessage = $"El paciente {user.FirstName} {user.LastName} tiene una cita agendada a esa hora, intenta en otro horario ";
                response.AlertIcon = "error";
                appointment.Response = response;
                return BadRequest(appointment);
            }
            var newAppointment = _mapper.Map<Appointment>(appointment);
            _context.Appointments.Add(newAppointment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAppointment", new { id = newAppointment.Id }, newAppointment);
        }

        // DELETE: api/Appointments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            if (_context.Appointments == null)
            {
                return NotFound();
            }
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
    }
}

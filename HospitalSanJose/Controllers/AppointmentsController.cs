using HospitalSanJose.Functions;
using HospitalSanJoseModel.DTO.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace HospitalSanJose.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly AppointmentsService _appointmentsService;
        private readonly DepartmentsService _departmentsService;
        private readonly DoctorsService _doctorsService;
        private readonly UsersService _usersService;
        public AppointmentsController(AppointmentsService appointmentsService, DepartmentsService departmentsService, DoctorsService doctorsService, UsersService usersService)
        {
            _appointmentsService = appointmentsService;
            _departmentsService = departmentsService;
            _doctorsService = doctorsService;
            _usersService = usersService;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentsService.GetList();
            return View(appointments);
        }

        // GET: Appointments
        public async Task<IActionResult> AppointmentsByDoctor()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var roles = HttpContext.Session.GetString("Roles");

            if (roles.Contains("Doctor"))
            {
            var appointments = await _appointmentsService.GetListByDoctor((int)id!);
            return View(appointments);

            }
            else
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
        }

        // GET: Appointments
        public async Task<IActionResult> AppointmentsByPacient()
        {
            var id = HttpContext.Session.GetInt32("UserId");
            var appointments = await _appointmentsService.GetListByPacient((int)id!);
            return View(appointments);
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var appointment = await _appointmentsService.GetById(id);
            if (appointment == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            if (appointment.Id == 0)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentsService.GetAssignedList();
            var departmentId = departments.FirstOrDefault()?.Id ?? 0;
            var doctors = await _doctorsService.GetListByDepartmentId(departmentId);
            var users = await _usersService.GetPacients();
            var doctorId = doctors.FirstOrDefault()?.Id ?? 0;
            var availableHours = await _appointmentsService.GetAttentionHours(DateTime.Now.ToString("yyyy-MM-dd"), doctorId);

            AppointmentCreate appointment = new()
            {
                AvailableHours = availableHours.Select(s => new SelectListItem()
                {
                    Value = s.ToString(),
                    Text = s.ToString()
                }).ToList(),
                Departments = departments.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.DepartmentName
                }).ToList(),
                Doctors = doctors.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.User.FirstName} {s.User.LastName} ({s.User.Username})"
                }).ToList(),
                Users = users.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.FirstName} {s.LastName} ({s.Username})"
                }).ToList(),
                AppointmentDate = DateTime.Now,
                User = users.FirstOrDefault(),
                Doctor = doctors.FirstOrDefault()
            };
            var PreviousUrl = Request.Headers["Referer"].ToString();
            HttpContext.Session.SetString("PreviousUrl", PreviousUrl);
            return View(appointment);
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoctorId,UserId,AppointmentDate,AppointmentTime,DepartmentId")] AppointmentCreate appointment)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var response = await _appointmentsService.Post(appointment);
            if (response != null && response.Response != null)
            {
                var departments = await _departmentsService.GetAssignedList();
                var doctors = await _doctorsService.GetListByDepartmentId((int)appointment.DepartmentId);
                var users = await _usersService.GetPacients();
                var availableHours = await _appointmentsService.GetAttentionHours(appointment.AppointmentDate.ToString("yyyy-MM-dd"),(int)appointment.DoctorId);
                var doctor = await _doctorsService.GetById((int)appointment.DoctorId);
                var user = await _usersService.GetById((int)appointment.UserId);

                AppointmentCreate appointmentResponse = new()
                {
                    AvailableHours = availableHours.Select(s => new SelectListItem()
                    {
                        Value = s.ToString(),
                        Text = s.ToString()
                    }).ToList(),
                    Departments = departments.Select(s => new SelectListItem()
                    {
                        Value = s.Id.ToString(),
                        Text = s.DepartmentName
                    }).ToList(),
                    Doctors = doctors.Select(s => new SelectListItem()
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.User.FirstName} {s.User.LastName} ({s.User.Username})"
                    }).ToList(),
                    Users = users.Select(s => new SelectListItem()
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.FirstName} {s.LastName} ({s.Username})"
                    }).ToList(),
                    AppointmentDate = appointment.AppointmentDate,
                    User = user,
                    Doctor = doctor,
                    Response = response.Response
                };

                return View(appointmentResponse);
            }
            var PreviousUrl = HttpContext.Session.GetString("PreviousUrl");

            return Redirect(PreviousUrl!);

        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var appointment = await _appointmentsService.GetById(id);
            if (appointment == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            if (appointment.Id == 0)
            {
                return NotFound();
            }

            return View(appointment);
        }

       

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var appointment = await _appointmentsService.GetById(id);
            if (appointment == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            if (appointment.Id == 0)
            {
                return NotFound();
            }
            var PreviousUrl = Request.Headers["Referer"].ToString();
            HttpContext.Session.SetString("PreviousUrl", PreviousUrl);
            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentsService.Delete(id);
            var PreviousUrl = HttpContext.Session.GetString("PreviousUrl");
            
            return Redirect(PreviousUrl!);
        }
        [HttpPost]
        public async Task<JsonResult> GetAvailableHoursByDoctor()
        {
            int doctorId = Convert.ToInt32(HttpContext.Request.Form["doctorId"].FirstOrDefault().ToString());
            var dateTime = HttpContext.Request.Form["appointmentDate"].FirstOrDefault().ToString();
            var availableHours = await _appointmentsService.GetAttentionHours(dateTime, doctorId);

            var jsonresult = availableHours;
            return Json(jsonresult);
        }

    }
}

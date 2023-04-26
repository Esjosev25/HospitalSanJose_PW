using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HospitalSanJose.Functions;
using HospitalSanJoseModel.DTO.UserRoles;
using HospitalSanJoseModel.DTO.Doctor;
using HospitalSanJoseModel;

namespace HospitalSanJose.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly DoctorsService _doctorsService;
        private readonly DepartmentsService _departmentsService;
        private readonly UsersService _usersService;
        public DoctorsController(DoctorsService doctorsService, DepartmentsService departmentsService, UsersService usersService)
        {
            _doctorsService = doctorsService;
            _departmentsService = departmentsService;
            _usersService = usersService;

        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorsService.GetList();
            if (doctors == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            return View(doctors);
        }

        // GET: Doctors/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var doctor = await _doctorsService.GetById(id);
            if (doctor == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }

            if (doctor.Id == 0)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // GET: Doctors/Create
        public async Task<IActionResult> Create()
        {

            var users = await _usersService.GetNonDoctors();

            DoctorCreate doctor = new()
            {

                Users = users.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Username
                }).ToList(),
                User = users.FirstOrDefault()
            };


            return View(doctor);
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Specialty,YearsOfExperience,Qualification")] DoctorCreate doctor)
        {
            var response = await _doctorsService.Post(doctor);
            if (response != null && response.Response != null)
            {
                return View(response);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Doctors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var doctor = await _doctorsService.GetById(id);
            if (doctor == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }

            if (doctor.Id == 0)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Specialty,YearsOfExperience,Qualification")] Doctor doctor)
        {
            if (!ModelState.IsValid)
                return View(doctor);
            var response = await _doctorsService.Put(doctor, id);
            if (response != null)
            {
                return View(response);

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Doctors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var doctor = await _doctorsService.GetById(id);
            if (doctor == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }

            if (doctor.Id == 0)
            {
                return NotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Delete/5        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _doctorsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
 
        [HttpPost]
        public async Task<JsonResult> GetDoctorsByDepartmentJson()
        {

            int departmentId = Convert.ToInt32(HttpContext.Request.Form["departmentId"].FirstOrDefault().ToString());
            var departments = await _doctorsService.GetListByDepartmentId(departmentId);

            var jsonresult = departments ;
            return Json(jsonresult);
        }
    }
}

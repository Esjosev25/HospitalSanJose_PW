using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalSanJose.Functions;
using HospitalSanJoseModel.DTO.DoctorDepartment;
using HospitalSanJoseModel.DTO.UserRoles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace HospitalSanJoseAPI.Controllers
{
    public class DoctorDepartmentsController : Controller
    {

        private readonly DoctorDepartmentsService _doctorDepartmentsService;
        private readonly DepartmentsService _departmentsService;
        private readonly DoctorsService _doctorsService;
        public DoctorDepartmentsController(DoctorDepartmentsService doctorDepartmentsService, DepartmentsService departmentsService, DoctorsService doctorsService)
        {
            _doctorDepartmentsService = doctorDepartmentsService;
            _departmentsService = departmentsService;
            _doctorsService = doctorsService;
        }

        // GET: DoctorDepartments
        public async Task<IActionResult> Index()
        {
            var doctorDeparments = await _doctorDepartmentsService.GetList();
            return View(doctorDeparments);
        }

        public async Task<IActionResult> DepartmentsByDoctor(int? id)
        {
            var doctorDepartmetns = await _doctorDepartmentsService.GetByDoctorId((int)id);
            if (doctorDepartmetns == null)
            {
                return RedirectToAction("401", "Error"); // redirect to the error page
            }
            return View(doctorDepartmetns);
        }

        // GET: DoctorDepartments/Create
        public async Task<IActionResult> Create()
        {
            var doctors = await _doctorsService.GetDoctorsWithRemainingDepartments();
            var doctorId = doctors.FirstOrDefault()?.Id ?? 0;
            var departments = await _departmentsService.GetAvailableDepartmentsForDoctor(doctorId);

            DoctorDepartmentCreate doctorDepartments = new()
            {
                Departments = departments.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.DepartmentName
                }).ToList(),
                Doctors = doctors.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.User.Username
                }).ToList(),
                Doctor = doctors.FirstOrDefault()
            };

            return View(doctorDepartments);
        }

        // POST: DoctorDepartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentId,DoctorId")] DoctorDepartmentCreate doctorDepartment)
        {

            ;

            var response = await _doctorDepartmentsService.Post(doctorDepartment);
            if (response != null && response.Response != null)
            {
                return View(response);
            }

            return RedirectToAction(nameof(Index));
        }


        // POST: DoctorDepartments/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _doctorDepartmentsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}

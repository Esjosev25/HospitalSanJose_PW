using Microsoft.AspNetCore.Mvc;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
    public class DepartmentsController : Controller
    {
        
        private readonly DepartmentsService _departmentsService;
        public DepartmentsController(DepartmentsService deparmentsService)
        {
            _departmentsService = deparmentsService;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentsService.GetList();
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var department = await _departmentsService.GetById(id);
            if (department.Id == 0)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentName,Description")] HospitalSanJoseModel.Department department)
        {
            var response = await _departmentsService.Post(department);
            if (response != null && response.Response != null)
            { 
                return View(response);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var department = await _departmentsService.GetById(id);
            if (department.Id == 0)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartmentName,Description")] HospitalSanJoseModel.Department department)
        {
            if (!ModelState.IsValid)
                return View(department);
            var response = await _departmentsService.Put(department, id);
            if (response != null)
            {
                return View(response);

            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var department = await _departmentsService.GetById(id);
            if (department.Id == 0)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

      
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly DoctorsService _doctorsService;
        public DoctorsController(DoctorsService doctorsService)
        {
             _doctorsService = doctorsService;
         
        }

        // GET: Doctors
        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorsService.GetList();
            return View(doctors);
        }

        //// GET: Doctors/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Doctors == null)
        //    {
        //        return NotFound();
        //    }

        //    var doctor = await _context.Doctors
        //        .Include(d => d.Department)
        //        .Include(d => d.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(doctor);
        //}

        //// GET: Doctors/Create
        //public IActionResult Create()
        //{
        //    var doc = _mapper.Map<HospitalSanJoseModel.Doctor>(_context.Doctors.FirstOrDefault(d=>d.Id==1));
        //    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id");
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        //// POST: Doctors/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserId,DepartmentId")] Doctor doctor)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(doctor);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", doctor.DepartmentId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", doctor.UserId);
        //    return View(doctor);
        //}

        //// GET: Doctors/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Doctors == null)
        //    {
        //        return NotFound();
        //    }

        //    var doctor = await _context.Doctors.FindAsync(id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", doctor.DepartmentId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", doctor.UserId);
        //    return View(doctor);
        //}

        //// POST: Doctors/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DepartmentId")] Doctor doctor)
        //{
        //    if (id != doctor.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(doctor);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DoctorExists(doctor.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", doctor.DepartmentId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", doctor.UserId);
        //    return View(doctor);
        //}

        //// GET: Doctors/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Doctors == null)
        //    {
        //        return NotFound();
        //    }

        //    var doctor = await _context.Doctors
        //        .Include(d => d.Department)
        //        .Include(d => d.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(doctor);
        //}

        //// POST: Doctors/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Doctors == null)
        //    {
        //        return Problem("Entity set 'HospitalDbContext.Doctors'  is null.");
        //    }
        //    var doctor = await _context.Doctors.FindAsync(id);
        //    if (doctor != null)
        //    {
        //        _context.Doctors.Remove(doctor);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DoctorExists(int id)
        //{
        //  return (_context.Doctors?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalSanJose.Models;

namespace HospitalSanJose.Controllers
{
    public class PersonalInfoesController : Controller
    {
        private readonly HospitalDbContext _context;

        public PersonalInfoesController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: PersonalInfoes
        public async Task<IActionResult> Index()
        {
            var hospitalDbContext = _context.PersonalInfos.Include(p => p.User);
            return View(await hospitalDbContext.ToListAsync());
        }

        // GET: PersonalInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PersonalInfos == null)
            {
                return NotFound();
            }

            var personalInfo = await _context.PersonalInfos
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personalInfo == null)
            {
                return NotFound();
            }

            return View(personalInfo);
        }

        // GET: PersonalInfoes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: PersonalInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Dpi,PhoneNumber1,PhoneNumber2,Birthdate,AddressLine1,AddressLine2,MaritalStatus,City")] PersonalInfo personalInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", personalInfo.UserId);
            return View(personalInfo);
        }

        // GET: PersonalInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PersonalInfos == null)
            {
                return NotFound();
            }

            var personalInfo = await _context.PersonalInfos.FindAsync(id);
            if (personalInfo == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", personalInfo.UserId);
            return View(personalInfo);
        }

        // POST: PersonalInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Dpi,PhoneNumber1,PhoneNumber2,Birthdate,AddressLine1,AddressLine2,MaritalStatus,City")] PersonalInfo personalInfo)
        {
            if (id != personalInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalInfoExists(personalInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", personalInfo.UserId);
            return View(personalInfo);
        }

        // GET: PersonalInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PersonalInfos == null)
            {
                return NotFound();
            }

            var personalInfo = await _context.PersonalInfos
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personalInfo == null)
            {
                return NotFound();
            }

            return View(personalInfo);
        }

        // POST: PersonalInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PersonalInfos == null)
            {
                return Problem("Entity set 'HospitalDbContext.PersonalInfos'  is null.");
            }
            var personalInfo = await _context.PersonalInfos.FindAsync(id);
            if (personalInfo != null)
            {
                _context.PersonalInfos.Remove(personalInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalInfoExists(int id)
        {
          return (_context.PersonalInfos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

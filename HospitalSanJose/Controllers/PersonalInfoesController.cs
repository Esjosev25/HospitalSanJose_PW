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
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
            var hospitalDbContext = _context.PersonalInfos.Include(p => p.User).Where(p => !p.User.Deleted);
            return View(await hospitalDbContext.ToListAsync());
        }

        // GET: PersonalInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
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
        public IActionResult Create(int? Id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
            if(Id!= null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == Id);
                if (user == null)
                    return NotFound();
                var users = new List<User>
                {
                    user
                   
                };
                users.AddRange(_context.Users.Where(u => u.Id != Id));
                ViewData["UserId"] = new SelectList(users, "Id", "Username");

            }
            else
            {

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            }
            return View();
        }

        // POST: PersonalInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Dpi,PhoneNumber1,PhoneNumber2,Birthdate,AddressLine1,AddressLine2,MaritalStatus,City")] PersonalInfo personalInfo)
        {

            var personalInfoDb = await _context.PersonalInfos.FirstOrDefaultAsync(u => u.UserId == personalInfo.UserId);
            if (personalInfoDb != null)
            {
                //Usuario ya posee informacion asociada
                var user = _context.Users.FirstOrDefault(u => u.Id == personalInfo.UserId);
                if (user == null)
                    return NotFound();
                var users = new List<User>
                {
                    user

                };
                users.AddRange(_context.Users.Where(u => u.Id != personalInfo.UserId));
                ViewData["UserId"] = new SelectList(users, "Id", "Username", personalInfo.UserId);
                return View(personalInfo);
            }

            var userDB = await _context.Users.FirstOrDefaultAsync(u => u.Id == personalInfo.UserId);
            if (userDB == null)
            {
                return NotFound();
            }
            personalInfo.User = userDB;

            personalInfo.User.Activated = true;
            _context.Add(personalInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        // GET: PersonalInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
            if (id == null || _context.PersonalInfos == null)
            {
                return NotFound();
            }

            var personalInfo = await _context.PersonalInfos.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == id);
            if (personalInfo == null)
            {
                return NotFound();
            }

            return View(personalInfo);
        }

        // POST: PersonalInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Username, [Bind("Id,UserId,Dpi,PhoneNumber1,PhoneNumber2,Birthdate,AddressLine1,AddressLine2,MaritalStatus,City")] PersonalInfo personalInfo)
        {
            if (id != personalInfo.Id)
            {
                return NotFound();
            }


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


            return View(personalInfo);
        }

        // GET: PersonalInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
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

        public async Task<IActionResult> EditByUserID(int id)
        {
            var personalInfo = await _context.PersonalInfos.Include(u => u.User).FirstOrDefaultAsync(u => u.UserId == id);
            if (personalInfo != null)
              return  Redirect($"/PersonalInfoes/Edit/{personalInfo.Id}");

            return RedirectToAction(nameof(Index));
        }
    }
}

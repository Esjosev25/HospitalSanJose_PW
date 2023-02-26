using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalSanJose.Models;
using HospitalSanJose.DTO;

namespace HospitalSanJose.Controllers
{
    public class UsersController : Controller
    {
        private readonly HospitalDbContext _context;

        public UsersController(HospitalDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
            return _context.Users != null ?
                          View(await _context.Users.Where(u => !u.Deleted).ToListAsync()) :
                          Problem("Entity set 'HospitalDbContext.Users'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/Login");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        // GET: Users/Create
        public IActionResult Create()
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] User user)
        {
            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            var userDB = _context.Users.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
            if (userDB != null && !userDB.Deleted)
                return View(user);

            // Call the Register method to set its properties
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
            // Add the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();
            await _context.SaveChangesAsync();
            //_logger.LogInformation($"Se registró el usuario {user.Username} con el correo {user.Email}");

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/Login");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();

            if (ModelState.IsValid)
            {
                var userDB = _context.Users.FirstOrDefault(u => (u.Username == user.Username || u.Email == user.Email) && u.Id != user.Id);
                if (userDB != null && !userDB.Deleted)
                    return View(user);
                try
                {
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/login");
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Deleted = true;
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJose.Models;
using HospitalSanJoseModel;
using AutoMapper;
using System.Collections.Generic;

namespace HospitalSanJose.Controllers
{
  public class UsersController : Controller
    {
        private readonly HospitalDbContext _context;
        private readonly IMapper _mapper;
        public UsersController(HospitalDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Users
        public IActionResult Index()
        {

            if (_context.Users == null)
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");

            var users = _mapper.Map<List<HospitalSanJoseModel.User>>((from u in _context.Users
                                                                      where !u.Deleted
                                                                      select u).ToList());

            return View(users);


        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {

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

            return View(_mapper.Map<HospitalSanJoseModel.User>(user));
        }



        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] Models.User user)
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
            var userResult = _mapper.Map<HospitalSanJoseModel.User>(user);
            return View(userResult);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] Models.User user)
        {
            
            if (id != user.Id)
            {
                return NotFound();
            }

            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            var userResult = _mapper.Map<HospitalSanJoseModel.User>(user);
            if (ModelState.IsValid)
            {
                var response = new HospitalSanJoseModel.Response();
                
                var username = _context.Users.FirstOrDefault(u => (u.Username == user.Username || u.Email == user.Email) && u.Id != user.Id);

                if (username != null && !username.Deleted)
                {

                    userResult.Response = response;
                    response.AlertMessage = "Correo/Email ya se encuentran registrados";

                    return View(userResult);
                }
                try
                {
                    var userDB = _context.Users.AsNoTracking().First(u => u.Id == id);
                    if ( user.Password != userDB.Password)
                    {
                        string salt = BCrypt.Net.BCrypt.GenerateSalt();
                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

                    }
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
            return View(userResult);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

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

        public async Task<IActionResult> BlockUser(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return NotFound();

            user.IsLocked = !user.IsLocked;
            _context.Users.Update(user);
            

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}

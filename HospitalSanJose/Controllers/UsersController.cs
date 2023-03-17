using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJose.Models;
using HospitalSanJoseModel;
using AutoMapper;

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
        public async Task<IActionResult> Index()
        {

            if (_context.Users == null)
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");

            IEnumerable<HospitalSanJoseModel.User> users = (from u in _context.Users
                                          where !u.Deleted
                                          select ReturnUserDTO(u)).ToList();

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

            return View(ReturnUserDTO(user));
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
            var userResult = ReturnUserDTO(user);
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

            if (ModelState.IsValid)
            {
                var response = new HospitalSanJoseModel.Response();
                var userResult = ReturnUserDTO(user);
                var username = _context.Users.FirstOrDefault(u => (u.Username == user.Username || u.Email == user.Email) && u.Id != user.Id);

                if (username != null && !username.Deleted)
                {

                    userResult.Response = response;
                    response.AlertMessage = "Correo/Email ya se encuentran registrados";

                    return View(userResult);
                }
                try
                {
                    //var userDB = _context.Users.FirstOrDefault(u => u.Id == id);
                    //if (userDB != null && user.Password != userDB.Password)
                    //{
                    //    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    //    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

                    //}
                    
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
            return View(ReturnUserDTO(user));
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

        //private User GetUserById()
        //{

        //}
        private static HospitalSanJoseModel.User ReturnUserDTO(Models.User user) => new()
        {
            Username = user.Username,
            Email = user.Email,
            Activated = user.Activated,
            Deleted = user.Deleted,
            FirstName = user.FirstName,
            Id = user.Id,
            Image = user.Image,
            IsLocked = user.IsLocked,
            LastName = user.LastName,
            Password = user.Password,
        };
    }
}

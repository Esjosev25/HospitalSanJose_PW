using HospitalSanJose.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanJose.Controllers
{
    public class ProfileController : Controller
    {
        private readonly HospitalDbContext _context;
        private readonly int _userId;
        public ProfileController(HospitalDbContext context)
        {
            _context = context;
    
        }



        // GET: Users/Profile
        public async Task<IActionResult> Index()
        {
            ViewBag.ActiveTab = "profile-overview";

            var id = 9;
            var user = await _context.Users.FindAsync(id);
            if (id == null || _context.PersonalInfos == null || user == null)
            {
                return NotFound();
            }

            var personalInfo = _context.PersonalInfos.FirstOrDefault((a) => a.UserId == id);
            if (personalInfo == null)
                personalInfo = new PersonalInfo
                {
                    User = user
                };
            else
                personalInfo.User = user;
            // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", personalInfo.UserId);
            return View(personalInfo);
        }

        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit()
        {
            ViewBag.ActiveTab = "profile-edit";

            var id = 9;
            var user = await _context.Users.FindAsync(id);
            if (id == null || _context.PersonalInfos == null || user == null)
            {
                return NotFound();
            }

            var personalInfo = _context.PersonalInfos.FirstOrDefault((a) => a.UserId == id);
            if (personalInfo == null)
                personalInfo = new PersonalInfo
                {
                    User = user
                };
            else
                personalInfo.User = user;
            // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", personalInfo.UserId);
            return View(personalInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PersonalInfo personalInfo)
        {
            ViewBag.ActiveTab = "profile-edit";
            var id = 9;
            var user = await _context.Users.FindAsync(id);
            var personalInfoDb = _context.PersonalInfos.FirstOrDefault((a) => a.UserId == id);
            if (user == null && personalInfoDb == null && id != user.Id)
            {
                return NotFound();
            }


            try
            {
                var userDB = _context.Users.FirstOrDefault(u => (u.Username == personalInfo.User.Username || u.Email == personalInfo.User.Email) && u.Id != id);
                if (userDB != null )
                {
                    return View(personalInfoDb);
                }
                personalInfoDb.User.Email = personalInfo.User.Email;
                personalInfoDb.User.Username = personalInfo.User.Username;
                personalInfoDb.PhoneNumber1 = personalInfo.PhoneNumber1;
                personalInfoDb.PhoneNumber2 = personalInfo.PhoneNumber2;
                personalInfoDb.MaritalStatus = personalInfo.MaritalStatus;
                personalInfoDb.City = personalInfo.City;
                personalInfoDb.AddressLine1 = personalInfo.AddressLine1;
                personalInfoDb.AddressLine2 = personalInfo.AddressLine2;

                _context.Update(personalInfoDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonalInformationExists(personalInfo.User.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
         


        }
        public  async Task<IActionResult> ChangePassword()
        {
            ViewBag.ActiveTab = "profile-change-password";
            var id = 9;
            var user = await _context.Users.FindAsync(id);
            if (id == null || _context.PersonalInfos == null || user == null)
            {
                return NotFound();
            }

            var personalInfo = _context.PersonalInfos.FirstOrDefault((a) => a.UserId == id);
            if (personalInfo == null)
                personalInfo = new PersonalInfo
                {
                    User = user
                };
            else
                personalInfo.User = user;
            // ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", personalInfo.UserId);
            return View(personalInfo);
        }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(string Password, string NewPassword, string ReNewPassword)
    {
      ViewBag.ActiveTab = "profile-edit";
      var id = 9;
      var user = await _context.Users.FindAsync(id);
      var personalInfoDb = _context.PersonalInfos.FirstOrDefault((a) => a.UserId == id);
      if (user == null || personalInfoDb == null)
      {
        return NotFound();
      }


      try
      {
                if (!NewPassword.Equals(ReNewPassword))
                {
                    // Agregar alerta en caso que no hagan match las psw

                    return View(personalInfoDb);
                }
                 if(!BCrypt.Net.BCrypt.Verify(Password, user.Password))
                {
                    return View(personalInfoDb);
                }
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
          
                user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword, salt);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
      catch (DbUpdateConcurrencyException)
      {
        
          throw;
        
      }



    }
        private bool PersonalInformationExists(int id)
        {
            return (_context.PersonalInfos?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}

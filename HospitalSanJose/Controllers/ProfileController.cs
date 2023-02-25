using HospitalSanJose.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalSanJose.Controllers
{
   [Route("users/profile/")]
    public class ProfileController : Controller
    {
        private readonly HospitalDbContext _context;

        public ProfileController(HospitalDbContext context)
        {
            _context = context;
        }

        
        
        // GET: Users/Profile
        public async Task<IActionResult> Index()
        {
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
        
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Profile/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Profile/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Profile/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Profile/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Profile/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Profile/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}

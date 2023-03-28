﻿using Microsoft.AspNetCore.Mvc;
using HospitalSanJoseModel;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
    public class UsersController : Controller
    {

        private readonly UsersService _userService;
        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {

            var users = await _userService.GetList();

            return View(users);


        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var user = await _userService.GetById(id);
            if (user.Id == 0)
            {
                return NotFound();
            }
            return View(user);
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
        public async Task<IActionResult> Create([Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] User user)
        {
            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            user.Password = user.Username.Trim();
            user.Response = null;
            if (user.Email == null || user.Username == null)
                return View(user);

            var response = await _userService.Post(user);
            if (response != null && response.Response != null)
            {
                return View(response);
            }



            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _userService.GetById(id);
            if (user.Id == 0)
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
            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            user.Password = user.Username.Trim();
            user.Response = null;
            if (ModelState.IsValid)
            {
                var response = await _userService.Put(user, id);
                if (response != null)
                {
                    return View(response);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var user = await _userService.GetById(id);
            if (user.Id == 0)
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
            await _userService.Delete(id);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> BlockUser(int id)
        {
            await _userService.ToggleBlockUser(id);

            return RedirectToAction(nameof(Index));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using HospitalSanJoseModel.DTO.UserRoles;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
    
    public class UserRolesController : Controller
    {

        private readonly IMapper _mapper;
        private readonly UserRolesService _userRolesService;
        private readonly UsersService _usersService;
        private readonly RolesService _rolesService;

        public UserRolesController( IMapper mapper, UserRolesService userRolesService, UsersService userService,RolesService rolesService )
        {
            
            _mapper = mapper;
            _userRolesService = userRolesService;
            _usersService = userService;
            _rolesService = rolesService;
        }

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            var userRoles = await _userRolesService.GetList();
            return View(userRoles);
        }


        // GET: UserRoles/Create
        public async Task<IActionResult> Create()
        {
            
            var users = await _usersService.GetUsersWithRemainingRoles();
            var userId = users.FirstOrDefault()?.Id ?? 0;
            var roles = await _rolesService.GetAvailableRolesForUser(userId);

            UserRolesCreate userRoles = new ()
            {
                Roles= roles.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                }).ToList(),
                Users = users.Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = s.Username
                }).ToList(),
                User= users.FirstOrDefault()
            };

            return View(userRoles);


   
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,UserId,RoleId")] UserRolesCreate userRole)
        //{
            
        //    return View(userRole);
        //}

        //// GET: UserRoles/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.UserRoles == null)
        //    {
        //        return NotFound();
        //    }

        //    var userRole = await _context.UserRoles.FindAsync(id);
        //    if (userRole == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", userRole.RoleId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userRole.UserId);
        //    return View(userRole);
        //}

        //// POST: UserRoles/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RoleId")] Models.UserRole userRole)
        //{
        //    if (id != userRole.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(userRole);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserRoleExists(userRole.Id))
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
        //    ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", userRole.RoleId);
        //    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userRole.UserId);
        //    return View(userRole);
        //}

        //// GET: UserRoles/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.UserRoles == null)
        //    {
        //        return NotFound();
        //    }

        //    var userRole = await _context.UserRoles
        //        .Include(u => u.Role)
        //        .Include(u => u.User)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userRole == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userRole);
        //}

        //// POST: UserRoles/Delete/5
        //[HttpPost]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _userRolesService.Delete(id);
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserRoleExists(int id)
        //{
        //  return (_context.UserRoles?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
        
    }
}

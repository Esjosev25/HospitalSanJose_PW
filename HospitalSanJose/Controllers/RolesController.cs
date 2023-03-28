using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HospitalSanJose.Models;
using HospitalSanJose.Functions;

namespace HospitalSanJose.Controllers
{
    public class RolesController : Controller
    {

        private readonly RolesService _rolesService;
        public RolesController(RolesService rolesService)
        {
            _rolesService = rolesService; 
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _rolesService.GetList();
            return View(roles);
        }






    }
}

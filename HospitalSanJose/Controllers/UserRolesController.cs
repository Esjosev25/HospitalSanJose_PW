using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // GET: UserRoles
        public async Task<IActionResult> RolesByUser(int? id)
        {
            var userRoles = await _userRolesService.GetUserRolesById((int)id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] UserRolesCreate userrole)
        {
        
        ;

            var response = await _userRolesService.Post(userrole);
            if (response != null && response.Response != null)
            {
                return View(response);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: UserRoles/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRolesService.Delete(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

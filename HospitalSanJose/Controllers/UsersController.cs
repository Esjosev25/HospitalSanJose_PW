using Microsoft.AspNetCore.Mvc;
using HospitalSanJoseModel;
using AutoMapper;
using HospitalSanJoseModel.Functions;

namespace HospitalSanJose.Controllers
{
  public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly UsersService _userService;
        public UsersController(UsersService userService, IMapper mapper, ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
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
        public async Task<IActionResult> Create([Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] HospitalSanJoseModel.User user)
        {
            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            user.Password = user.Username.Trim();
            
            if (user.Email == null || user.Username ==null)
                return View(user);

            await _userService.Post(user);
            _logger.LogInformation($"Se registró el usuario {user.Username} con el correo {user.Email}");

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var name = HttpContext.Session.GetString("Username");
            var userId = HttpContext.Session.GetInt32("UserId");
            if (name == null || userId == null)
                return Redirect("/auth/Login");


            var user = await _userService.GetById(id);
            if (user.Id == 0)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Password,NeedChangePassword,Email,FirstName,LastName,Image,Deleted,Activated,Username,IsLocked")] User user)
        {
            user.Email = user.Email.Trim();
            user.Username = user.Username.Trim();
            user.Password = user.Username.Trim();

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
            _logger.LogInformation($"Se eliminó el usuario con id: {id}");
            return RedirectToAction(nameof(Index));
        }

     

        public async Task<IActionResult> BlockUser(int id)
        {
            await _userService.ToggleBlockUser(id);

            return RedirectToAction(nameof(Index));
        }

    }
}

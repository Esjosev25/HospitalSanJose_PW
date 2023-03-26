﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;
using System.Linq;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly HospitalDbContext _context;
        public UsersController(HospitalDbContext context, IMapper mapper, ILogger<UsersController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var users = _mapper.Map<IEnumerable<HospitalSanJoseModel.User>>(await (from u in _context.Users
                                                                                   where !u.Deleted
                                                                                   select u).ToListAsync());
            return Ok(users);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = _mapper.Map<HospitalSanJoseModel.User>(await _context.Users.FindAsync(id));

            if (user != null)
            {
            return Ok(user);
            }

                return NotFound();
        }

       
        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, HospitalSanJoseModel.User user)
        {
            if (id != user.Id)
            {

                return BadRequest(new
                {
                    message = "Parametro por ruta y el id de usuario deben de ser igual",
                    error = true
                });
            }
            var userDB = _context.Users.FirstOrDefault(u => u.Id != user.Id && (u.Username == user.Username || u.Email == user.Email ));

            if (userDB != null)
            {
                var response = new HospitalSanJoseModel.Response();
                response.AlertMessage = "Correo o Usuario ya existen, intenta con uno nuevo";
                response.AlertIcon = "error";
                user.Response = response;
                return BadRequest(user);
            }
            var userPassword = _context.Users.AsNoTracking().First(u => u.Id == id);
            if (user.Password != userPassword.Password)
            {
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

            }
            var changedUser= _mapper.Map<User>(user);
            _context.Entry(changedUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Block/{id}")]
        public async Task<IActionResult> ToggleBlockUser(int id)
        {

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                user.IsLocked = !user.IsLocked;
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HospitalSanJoseModel.User>> PostUser(HospitalSanJoseModel.UserCreateDTO user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'HospitalDbContext.Users'  is null.");
            }
            var userDB = _context.Users.FirstOrDefault(u => u.Username == user.Username || u.Email == user.Email);
             
            if (userDB != null)
            {
                return BadRequest(new
                {
                    message = "Correo o Usuario ya existen, intenta con uno nuevo",
                    error = true
                });
            }

            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password, salt);

            var newUser = _mapper.Map<User>(user);
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Se registró el usuario {user.Username} con el correo {user.Email}");
            return CreatedAtAction("GetUser", new { id = newUser.Id }, newUser);
        }

        //// DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.Deleted = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
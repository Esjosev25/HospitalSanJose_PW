using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalSanJoseAPI.Models;
using AutoMapper;
using DTO = HospitalSanJoseModel.DTO.Department;

namespace HospitalSanJoseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly HospitalDbContext _context;
        public DepartmentsController(HospitalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HospitalSanJoseModel.Department>>> GetDepartments()
        {
            if (_context.Departments == null)
            {
                return NotFound();
            }
            var departments = _mapper.Map<IEnumerable<HospitalSanJoseModel.Department>>(await _context.Departments.ToListAsync());
            return Ok(departments);
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HospitalSanJoseModel.Department>> GetDepartment(int id)
        {
            if (_context.Departments == null)
            {
                return NotFound();
            }
            var department = _mapper.Map<HospitalSanJoseModel.Department>(await _context.Departments.FindAsync(id));

            if (department == null)
            {
                return NotFound();
            }

            return Ok(department);
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment(int id, HospitalSanJoseModel.Department department)
        {
                var response = new HospitalSanJoseModel.Response();
            if (id != department.Id)
            {
                response.AlertMessage = "Parametro por ruta y el id de personalInfo deben de ser igual";
                response.AlertIcon = "error";
                department.Response = response;
                return BadRequest(department);
            }
            var departmentDb = await _context.Departments.FirstOrDefaultAsync(r => r.DepartmentName.Equals(department.DepartmentName));
           
            if (departmentDb != null)
            {
                response.AlertMessage = "Ya existe un departamento con ese nombre";
                response.AlertIcon = "warning";
                department.Response = response;
                return BadRequest(department);
            }
            var updatedDepartment = _mapper.Map<Department>(department);
            _context.Entry(updatedDepartment).State = EntityState.Modified;


            await _context.SaveChangesAsync();


            return NoContent();
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HospitalSanJoseModel.Department>> PostDepartment(DTO.DepartmentCreate department)
        {
            if (_context.Departments == null)
            {
                return Problem("Entity set 'HospitalDbContext.Departments'  is null.");
            }
            var departmentDb = await _context.Departments.FirstOrDefaultAsync(r => r.DepartmentName.Equals(department.DepartmentName));
            var response = new HospitalSanJoseModel.Response();
            if (departmentDb!= null)
            {
                response.AlertMessage = "Ya existe un departamento con ese nombre";
                response.AlertIcon = "warning";
                department.Response = response;
                return BadRequest(department);
            }

            var newDepartment = _mapper.Map<Department>(department);
            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDepartment", new { id = newDepartment.Id }, newDepartment);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            if (_context.Departments == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TeleperformanceTask.Data;
using TeleperformanceTask.Models;

namespace TeleperformanceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // All endpoints require authentication
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET api/employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Allow admins to view all employees, otherwise only view employees created by the current user
            var employees = User.IsInRole("ADMIN")
                ? await _context.Employees.ToListAsync()
                : await _context.Employees.Where(e => e.UserId == userId).ToListAsync();

            return Ok(employees);
        }

        // GET api/employees/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            // Allow admins to view any employee, otherwise only allow viewing employees created by the current user
            if (employee.UserId != userId && !User.IsInRole("ADMIN"))
                return Forbid();

            return Ok(employee);
        }

        // POST api/employees
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromForm] Employee employee)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Ensure that normal users can only create employees with their own UserId
                if (!User.IsInRole("ADMIN") && employee.UserId != userId)
                    return Forbid();
                if (employee.ImagePath == "" || employee.ImagePath == null) employee.ImagePath = "https://letsenhance.io/static/03620c83508fc72c6d2b218c7e304ba5/11499/UpscalerAfter.jpg";
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
            catch (Exception ex) { throw; }

        }

        // PUT api/employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (employee.ImagePath == "" || employee.ImagePath == null) employee.ImagePath = "https://letsenhance.io/static/03620c83508fc72c6d2b218c7e304ba5/11499/UpscalerAfter.jpg";
            var existingEmployee = await _context.Employees.FindAsync(id);

            if (existingEmployee == null)
                return NotFound();

            // Allow admins to update any employee, otherwise only allow updating employees created by the current user
            if (existingEmployee.UserId != userId && !User.IsInRole("ADMIN"))
                return Forbid();


            existingEmployee.UserName = employee.UserName;
            existingEmployee.Email = employee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber;
            existingEmployee.EducationLevel = employee.EducationLevel;
            existingEmployee.ImagePath = employee.ImagePath;

            _context.Entry(existingEmployee).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            // Allow admins to delete any employee, otherwise only allow deleting employees created by the current user
            if (employee.UserId != userId && !User.IsInRole("ADMIN"))
                return Forbid();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

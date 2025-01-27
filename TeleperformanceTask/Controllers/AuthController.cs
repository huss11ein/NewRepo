using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeleperformanceTask.Models;
using TeleperformanceTask.Services;

namespace TeleperformanceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ITokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                Role = model.Role
            };
            var userEx = await _userManager.FindByEmailAsync(model.Email);
            if (userEx != null) return BadRequest("Email Already Exist");


            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                var roleCreationResult = await _roleManager.CreateAsync(new IdentityRole(model.Role));
                if (!roleCreationResult.Succeeded)
                    return BadRequest($"Failed to create the role {model.Role}: {string.Join(", ", roleCreationResult.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token, userId = user.Id, userName=user.UserName });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);


                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Unauthorized();

                return Ok(new { Token = _tokenService.GenerateToken(user), userId = user.Id, userName = user.UserName });

            }
            catch (Exception ex)
            {
                throw;

            }

        }
    }
}
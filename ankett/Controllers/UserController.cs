using ankett.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ankett.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return BadRequest(new { message = "Username already exists!" });
            }

            var user = new User
            {
                Username = registerDto.Username,
                PasswordHash = registerDto.Password,
                Role = registerDto.Role
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "User registered successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving to the database.", error = ex.Message });
            }
        }
            [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || user.PasswordHash != loginDto.Password)
                return Unauthorized();

            return Ok(new { user.Id, user.Role });
        }
    }
        public class RegisterDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public UserRole Role { get; set; }
        }

        public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

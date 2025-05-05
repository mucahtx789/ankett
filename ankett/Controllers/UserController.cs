using ankett.Models;
using ankett.Models.Dto;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ankett.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public UserController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                return BadRequest(new { Success = false, message = "Username is used." });
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
                return Ok(new { Success = true, message = "User registration successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, message = "An error occurred while saving the database.", error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var recaptchaValid = await VerifyRecaptchaAsync(loginDto.RecaptchaToken);
            if (!recaptchaValid)
                return BadRequest(new { message = "reCAPTCHA doğrulaması başarısız." });


            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || user.PasswordHash != loginDto.Password)
                return Unauthorized(new { message = "Invalid username or password." });

            var token = GenerateJwtToken(user.Id, user.Role == 0 ? "Admin" : "Employee");
            return Ok(new { user.Id, user.Role, token });
        }
        // JWT Token oluşturma işlemi
        private string GenerateJwtToken(int userId, string role)
        {

            var jwtKey = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException("Jwt:Key", "JWT Anahtarı 'Jwt:Key' yapılandırma dosyasından alınamıyor.");
            }
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        //recaptcha
        private async Task<bool> VerifyRecaptchaAsync(string recaptchaToken)
        {
            var secretKey = _config["Recaptcha:SecretKey"];
            using var httpClient = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("secret", secretKey),
        new KeyValuePair<string, string>("response", recaptchaToken)
    });

            var response = await httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
            var responseString = await response.Content.ReadAsStringAsync();
            var recaptchaResult = System.Text.Json.JsonSerializer.Deserialize<RecaptchaResponse>(responseString);
          
            return recaptchaResult?.Success ?? false;
        }

        

    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleJwtApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private const string SecretKey = "ThisIsASecretKeyForJwtGeneration";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // Hardcoded credentials for demo
            var isAdmin = dto.Username == "admin" && dto.Password == "admin123";
            var isUser = dto.Username == "user" && dto.Password == "user123";

            if (!isAdmin && !isUser)
                return Unauthorized("Invalid credentials");

            var role = isAdmin ? "Admin" : "User";

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, dto.Username),
                new(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { token = jwt });
        }

        [HttpGet("whoami")]
        [Authorize]
        public IActionResult WhoAmI()
        {
            var username = User.Identity?.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return Ok(new { username, role });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly() => Ok("Welcome, Admin!");

        [HttpGet("user")]
        [Authorize(Roles = "User,Admin")]
        public IActionResult UserOrAdmin() => Ok("Hello User or Admin!");
    }

    public class LoginDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}

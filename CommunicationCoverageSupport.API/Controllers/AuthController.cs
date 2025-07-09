// File: CommunicationCoverageSupport/API/Controllers/AuthController.cs
using CommunicationCoverageSupport.BLL.Services.Auth;
using CommunicationCoverageSupport.Models.DTOs.Auth;
using CommunicationCoverageSupport.Models.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var newUser = new UserRegister
            {
                Username = dto.Username,
                Password = dto.Password,
                Role = "user"
            };

            var success = await _authService.RegisterAsync(newUser);
            if (!success)
                return BadRequest("Registration failed.");

            return Ok("User registered successfully.");
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegisterDto dto)
        {
            var newAdmin = new UserRegister
            {
                Username = dto.Username,
                Password = dto.Password,
                Role = "admin"
            };

            var success = await _authService.RegisterAsync(newAdmin);
            if (!success)
                return BadRequest("Registration failed.");

            return Ok("Admin registered successfully.");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);
            if (response == null)
                return Unauthorized("Invalid credentials.");

            return Ok(response);
        }
        [HttpGet("test_login")]
        [AllowAnonymous]
        public async Task<IActionResult> TestLogin()
        {
            var response = await _authService.TestLoginAsync();
            if (response == null)
                return Unauthorized("Invalid credentials.");

            return Ok(response);
        }
    }
}

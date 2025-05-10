using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace CommunicationCoverageSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result)
                return BadRequest("User registration failed.");

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            if (response == null)
                return Unauthorized("Invalid username or password.");

            return Ok(response);
        }

        [HttpGet("check-role")]
        [Authorize]
        public IActionResult CheckRole()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(new
            {
                role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                claims
            });
        }
    }
}

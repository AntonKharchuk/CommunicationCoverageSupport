using CommunicationCoverageSupport.BLL.Services.SimCards;
using CommunicationCoverageSupport.Models.DTOs.SimCards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CommunicationCoverageSupport.BLL.Services;

namespace CommunicationCoverageSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SimCardController : ControllerBase
    {
        private readonly ISimCardService _service;
        private readonly IAuthService _authService;

        public SimCardController(ISimCardService service, IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return int.Parse(claim!.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserId();

            if (await _authService.IsAdminAsync(userId))
            {
                var all = await _service.GetAllAsync();
                return Ok(all);
            }

            var own = await _service.GetAllByUserAsync(userId);
            return Ok(own);
        }

        [HttpGet("{iccid}")]
        public async Task<IActionResult> GetById(string iccid)
        {
            var card = await _service.GetByIccidAsync(iccid);
            return card == null ? NotFound() : Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSimCardDto dto)
        {
            var userId = GetUserId();
            var result = await _service.CreateAsync(dto, userId);
            return result ? Ok("Created") : Unauthorized("Access denied or validation failed");
        }

        [HttpDelete("{iccid}")]
        public async Task<IActionResult> Delete(string iccid)
        {
            var userId = GetUserId();
            var result = await _service.DeleteAsync(iccid, userId);
            return result ? Ok("Deleted") : Unauthorized("Not authorized or not found");
        }
    }
}

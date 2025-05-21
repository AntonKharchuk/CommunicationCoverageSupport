using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.BLL.Services.SimCards;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimCardDrainController : ControllerBase
    {
        private readonly ISimCardDrainService _service;

        public SimCardDrainController(ISimCardDrainService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{iccid}")]
        public async Task<IActionResult> GetByIccid(string iccid)
        {
            var card = await _service.GetByIccidAsync(iccid);
            return card is null ? NotFound() : Ok(card);
        }

        [HttpGet("full/{iccid}")]
        public async Task<IActionResult> GetFullInfoByIccid(string iccid)
        {
            var full = await _service.GetFullInfoByIccidAsync(iccid);
            return full is null ? NotFound() : Ok(full);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] string iccid, [FromQuery] string imsi, [FromQuery] string msisdn, [FromQuery] byte kIndId)
        {
            var result = await _service.DeleteAsync(iccid, imsi, msisdn, kIndId);
            return result ? Ok("Deleted.") : NotFound();
        }
    }
}

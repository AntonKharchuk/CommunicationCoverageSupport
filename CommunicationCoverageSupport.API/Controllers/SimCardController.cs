using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimCardController : ControllerBase
    {
        private readonly ISimCardService _service;

        public SimCardController(ISimCardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SimCardDto>>> GetAll()
        {
            var cards = await _service.GetAllAsync();
            return Ok(cards);
        }

        [HttpGet("{iccid}")]
        public async Task<ActionResult<SimCardDto>> GetByIccid(string iccid)
        {
            var card = await _service.GetByIccidAsync(iccid);
            if (card == null)
                return NotFound();
            return Ok(card);
        }
        [HttpGet("full-info/{iccid}")]
        public async Task<IActionResult> GetFullInfo(string iccid)
        {
            var result = await _service.GetFullInfoByIccidAsync(iccid);
            if (result == null)
                return NotFound("SIM card not found.");

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SimCardDto dto)
        {
            var success = await _service.CreateAsync(dto);
            if (!success) return BadRequest("Creation failed.");
            return Ok("Sim card created.");
        }

        [HttpPut("{iccid}")]
        public async Task<IActionResult> Update(string iccid, SimCardDto dto)
        {
            if (iccid != dto.Iccid) return BadRequest("ICCID mismatch.");
            var success = await _service.UpdateAsync(dto);
            if (!success) return NotFound("Sim card not found.");
            return Ok("Sim card updated.");
        }

        [HttpDelete("{iccid}")]
        public async Task<IActionResult> Delete(string iccid)
        {
            var success = await _service.DeleteAsync(iccid);
            if (!success) return NotFound("Sim card not found.");
            return Ok("Sim card deleted.");
        }
    }
}

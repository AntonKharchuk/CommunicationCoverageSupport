using CommunicationCoverageSupport.BLL.Services.SimCards;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimCardController : ControllerBase
    {
        private readonly ISimCardService _service;

        public SimCardController(ISimCardService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{iccid}")]
        public async Task<IActionResult> GetByIccid(string iccid)
        {
            var result = await _service.GetByIccidAsync(iccid);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("full/{iccid}")]
        public async Task<IActionResult> GetFullInfoByIccid(string iccid)
        {
            var result = await _service.GetFullInfoByIccidAsync(iccid);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SimCardDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return created ? Ok("Sim card created.") : BadRequest("Failed to create sim card.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(SimCardDto dto)
        {
            var updated = await _service.UpdateAsync(dto);
            return updated ? Ok("Sim card updated.") : NotFound();
        }

        [HttpPost("drain")]
        public async Task<IActionResult> Drain([FromQuery] string iccid, [FromQuery] string imsi, [FromQuery] string msisdn, [FromQuery] byte kIndId)
        {
            var message = await _service.DrainAsync(iccid, imsi, msisdn, kIndId);
            return Ok(message);
        }
    }
}

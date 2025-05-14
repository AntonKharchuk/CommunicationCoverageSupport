using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MsisdnController : ControllerBase
    {
        private readonly IMsisdnService _service;

        public MsisdnController(IMsisdnService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<MsisdnDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{msisdn}")]
        public async Task<ActionResult<MsisdnDto>> GetByMsisdn(string msisdn)
        {
            var item = await _service.GetByMsisdnAsync(msisdn);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MsisdnDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result) return BadRequest("Creation failed.");
            return Ok("MSISDN created.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MsisdnDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (!result) return NotFound("Update failed.");
            return Ok("MSISDN updated.");
        }

        [HttpDelete("{msisdn}")]
        public async Task<IActionResult> Delete(string msisdn)
        {
            var result = await _service.DeleteAsync(msisdn);
            if (!result) return NotFound("Delete failed.");
            return Ok("MSISDN deleted.");
        }
    }
}

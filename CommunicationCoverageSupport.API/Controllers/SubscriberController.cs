using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberService _service;

        public SubscriberController(ISubscriberService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<SubscriberDto>>> GetAll()
        {
            var subscribers = await _service.GetAllAsync();
            return Ok(subscribers);
        }

        [HttpGet("{msisdn}")]
        public async Task<ActionResult<SubscriberDto>> GetByMsisdn(string msisdn)
        {
            var subscriber = await _service.GetByMsisdnAsync(msisdn);
            if (subscriber == null)
                return NotFound();
            return Ok(subscriber);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubscriberDto dto)
        {
            var success = await _service.CreateAsync(dto);
            if (!success)
                return BadRequest("Creation failed. Please ensure MSISDN and IMSI are available.");
            return Ok("Subscriber created.");
        }

        [HttpDelete("{msisdn}")]
        public async Task<IActionResult> Delete(string msisdn)
        {
            var success = await _service.DeleteAsync(msisdn);
            if (!success)
                return NotFound("Subscriber not found or could not be deleted.");
            return Ok("Subscriber deleted.");
        }
    }
}

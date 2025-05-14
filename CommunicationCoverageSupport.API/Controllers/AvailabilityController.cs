using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _service;

        public AvailabilityController(IAvailabilityService service)
        {
            _service = service;
        }

        [HttpGet("imsi-free")]
        public async Task<ActionResult<List<FreeImsiDto>>> GetFreeImsis()
        {
            var result = await _service.GetFreeImsisAsync();
            return Ok(result);
        }

        [HttpGet("msisdn-free")]
        public async Task<ActionResult<List<FreeMsisdnDto>>> GetFreeMsisdns()
        {
            var result = await _service.GetFreeMsisdnsAsync();
            return Ok(result);
        }
    }
}

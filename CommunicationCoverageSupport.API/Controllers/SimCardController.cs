using CommunicationCoverageSupport.BLL.Services.SimCards;
using CommunicationCoverageSupport.Models.DTOs;
using CommunicationCoverageSupport.Models.DTOs.InfoDTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "user,admin")]
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

        [HttpGet("imsi/{imsi}")]
        public async Task<IActionResult> GetByImsi(string imsi)
        {
            var result = await _service.GetByImsiAsync(imsi);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("full/{iccid}")]
        public async Task<IActionResult> GetFullInfoByIccid(string iccid)
        {
            var result = await _service.GetFullInfoByIccidAsync(iccid);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(SimCardDto dto)
        {
            var (statusCode, message) = await _service.CreateAsync(dto);
            return StatusCode(statusCode, message);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(SimCardDto dto)
        {
            var (statusCode, message) = await _service.UpdateAsync(dto);
            return StatusCode(statusCode, message);
        }

        [HttpPatch("installed")]
        public async Task<IActionResult> UpdateInstalledState([FromBody] UpdateInstalledRequestDto request)
        {
            var (statusCode, message) = await _service.UpdateInstalledStateAsync(request.PrimaryKey, request.Installed);
            return StatusCode(statusCode, message);
        }


        [HttpPost("drain")]
        public async Task<IActionResult> Drain(SimCardPrimaryKeyDto simCardPrimaryKeyDto)
        {
            var message = await _service.DrainAsync(simCardPrimaryKeyDto.Iccid, simCardPrimaryKeyDto.Imsi,
                simCardPrimaryKeyDto.Msisdn, simCardPrimaryKeyDto.KIndId);
            return Ok(message);
        }
    }
}

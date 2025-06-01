using CommunicationCoverageSupport.BLL.Services.TransportKeys;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class TransportKeyController : ControllerBase
    {
        private readonly ITransportKeyService _service;

        public TransportKeyController(ITransportKeyService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var keys = await _service.GetAllAsync();
            return Ok(keys);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(byte id)
        {
            var key = await _service.GetByIdAsync(id);
            return key is null ? NotFound() : Ok(key);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransportKeyDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result ? Ok("Created successfully") : BadRequest("Failed to create");
        }

        [HttpPut]
        public async Task<IActionResult> Update(TransportKeyDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return result ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            var result = await _service.DeleteAsync(id);
            return result ? Ok("Deleted successfully") : NotFound();
        }
    }
}

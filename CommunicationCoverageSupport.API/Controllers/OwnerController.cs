using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerService _service;

        public OwnerController(IOwnerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<OwnerDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnerDto>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OwnerDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result) return BadRequest("Creation failed.");
            return Ok("Owner created.");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OwnerDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            if (!result) return NotFound("Update failed.");
            return Ok("Owner updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound("Delete failed.");
            return Ok("Owner deleted.");
        }
    }
}

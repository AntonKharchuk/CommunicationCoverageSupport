using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class AccController : ControllerBase
    {
        private readonly IAccService _service;

        public AccController(IAccService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<AccDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccDto>> GetById(byte id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            var success = await _service.CreateAsync(name);
            if (!success) return BadRequest("Creation failed.");
            return Ok("ACC created.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(byte id, [FromBody] string name)
        {
            var success = await _service.UpdateAsync(id, name);
            if (!success) return NotFound("Update failed.");
            return Ok("ACC updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound("Delete failed.");
            return Ok("ACC deleted.");
        }
    }
}

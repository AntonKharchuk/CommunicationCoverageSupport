using CommunicationCoverageSupport.BLL.Services;
using CommunicationCoverageSupport.Models.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace CommunicationCoverageSupport.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtworkController : ControllerBase
    {
        private readonly IArtworkService _service;

        public ArtworkController(IArtworkService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArtworkDto>>> GetAll()
        {
            var artworks = await _service.GetAllAsync();
            return Ok(artworks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtworkDto>> GetById(byte id)
        {
            var artwork = await _service.GetByIdAsync(id);
            if (artwork == null)
                return NotFound();
            return Ok(artwork);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] string name)
        {
            var result = await _service.CreateAsync(name);
            if (!result) return BadRequest("Creation failed.");
            return Ok("Artwork created.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(byte id, [FromBody] string name)
        {
            var result = await _service.UpdateAsync(id, name);
            if (!result) return NotFound("Update failed.");
            return Ok("Artwork updated.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result) return NotFound("Delete failed.");
            return Ok("Artwork deleted.");
        }
    }
}

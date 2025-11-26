using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostanskiUrediController : ControllerBase
    {
        private readonly IPostanskiUredService _postanskiUredService;

        public PostanskiUrediController(IPostanskiUredService postanskiUredService)
        {
            _postanskiUredService = postanskiUredService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<GetPostanskiUredDto>>> GetAll()
        {
            return Ok(await _postanskiUredService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetPostanskiUredDto>> GetById(Guid id)
        {
            var uredDto = await _postanskiUredService.GetById(id);
            if (uredDto == null) return NotFound();

            return Ok(uredDto);
        }

        [HttpPost]
        public async Task<ActionResult<GetPostanskiUredDto>> Add([FromBody] UpsertPostanskiUredDto dto)
        {
            try
            {
                var created = await _postanskiUredService.Add(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetPostanskiUredDto>> Update([FromRoute] Guid id, [FromBody] UpsertPostanskiUredDto dto)
        {
            var updated = await _postanskiUredService.Update(id, dto);
            if (updated == null) return NotFound();

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _postanskiUredService.Delete(id);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}
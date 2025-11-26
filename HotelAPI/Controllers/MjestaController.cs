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
    public class MjestaController : ControllerBase
    {
        private readonly IMjestoService _mjestoService;

        public MjestaController(IMjestoService mjestoService)
        {
            _mjestoService = mjestoService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<GetMjestoDto>>> GetAll()
        {
            return Ok(await _mjestoService.GetAll());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetMjestoDto>> GetById(Guid id)
        {
            var mjestoDto = await _mjestoService.GetById(id);
            if (mjestoDto == null) return NotFound();
            return Ok(mjestoDto);
        }

        [HttpPost]
        public async Task<ActionResult<GetMjestoDto>> Add(UpsertMjestoDto mjestoDto)
        {
            try
            {
                var mjesto = await _mjestoService.Add(mjestoDto);
                return Ok(mjesto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GetMjestoDto>> Update(Guid id, UpsertMjestoDto dto)
        {
            var mjesto = await _mjestoService.Update(id, dto);
            if (mjesto == null) return NotFound();
            return Ok(mjesto);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mjestoService.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}
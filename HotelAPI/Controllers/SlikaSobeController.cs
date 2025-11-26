using HotelAPI.Models.Dto;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/Hoteli/sobe/{sobaId:guid}/slike")]
    [ApiController]
    public class SlikaSobeController(ISlikaSobeService slikaService) : ControllerBase
    {
        private readonly ISlikaSobeService slikaService = slikaService;
        private readonly ILogger<HoteliController> logger;

        [HttpGet]
        public async Task<ActionResult<List<GetSlikaSobeDto>>> GetAll(Guid sobaId)
        {
            return Ok(await slikaService.GetAllForSoba(sobaId));
        }

        [HttpGet("{slikaId:guid}")]
        public async Task<ActionResult<GetSlikaSobeDto>> GetById(Guid sobaId, Guid slikaId)
        {
            var slika = await slikaService.GetById(sobaId, slikaId);
            if (slika is null) return NotFound();
            return Ok(slika);
        }

        [HttpPost]
        public async Task<ActionResult<GetSlikaSobeDto>> Add(Guid sobaId, [FromForm] UpsertSlikaSobeDto dto)
        {
            var slika = await slikaService.Add(sobaId, dto);
            return Ok(slika);
        }

        [HttpPut("{slikaId:guid}")]
        public async Task<ActionResult<GetSlikaSobeDto>> Update(Guid sobaId, Guid slikaId, UpsertSlikaSobeDto dto)
        {
            var slika = await slikaService.Update(sobaId, slikaId, dto);
            if (slika is null) return NotFound();
            return Ok(slika);
        }

        [HttpDelete("{slikaId:guid}")]
        public async Task<IActionResult> Delete(Guid sobaId, Guid slikaId)
        {
            var result = await slikaService.Delete(sobaId, slikaId);
            if (!result) return NotFound();
            return Ok();
        }
    }
}

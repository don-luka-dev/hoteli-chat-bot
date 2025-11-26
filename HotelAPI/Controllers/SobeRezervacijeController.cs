using HotelAPI.Models.Dto;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel.Services;

namespace HotelAPI.Controllers
{
    [Route("api/sobe/{sobaId:guid}/rezervacije")]
    [ApiController]
    public class SobeRezervacijeController : ControllerBase
    {
        private readonly ISobaRezervacijaService _rezervacijaService;
        private readonly ILogger<HoteliController> logger;

        public SobeRezervacijeController(ISobaRezervacijaService rezervacijaService)
        {
            _rezervacijaService = rezervacijaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetRezervacijaDto>>> GetAll(Guid sobaId)
        {
            return Ok(await _rezervacijaService.GetAllForSoba(sobaId));
        }

        [HttpGet("{rezervacijaId:guid}")]
        public async Task<ActionResult<GetRezervacijaDto>> GetById(Guid sobaId, Guid rezervacijaId)
        {
            var rezervacija = await _rezervacijaService.GetById(sobaId, rezervacijaId);
            if (rezervacija == null) return NotFound();
            return Ok(rezervacija);
        }

        [HttpPost]
        public async Task<ActionResult<GetRezervacijaDto>> Add(Guid sobaId, UpsertRezervacijaDto rezervacijaDto)
        {
            try
            {
                var rezultat = await _rezervacijaService.Add(sobaId, rezervacijaDto);
                return Ok(rezultat);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{rezervacijaId:guid}")]
        public async Task<ActionResult<GetRezervacijaDto>> Update(Guid sobaId, Guid rezervacijaId, UpsertRezervacijaDto rezervacijaDto)
        {
            var rezultat = await _rezervacijaService.Update(sobaId, rezervacijaId, rezervacijaDto);
            if (rezultat == null) return NotFound();
            return Ok(rezultat);
        }

        [HttpDelete("{rezervacijaId:guid}")]
        public async Task<IActionResult> Delete(Guid sobaId, Guid rezervacijaId)
        {
            var success = await _rezervacijaService.Delete(sobaId, rezervacijaId);
            if (!success) return NotFound();
            return Ok();
        }
    }
}

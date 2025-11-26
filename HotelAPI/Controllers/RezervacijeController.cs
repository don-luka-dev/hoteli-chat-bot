using HotelAPI.Models.Dto;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/rezervacije")]
    [ApiController]
    public class RezervacijeController : Controller
    {
        private readonly IRezervacijaService _rezervacijaService;

        public RezervacijeController(IRezervacijaService rezervacijaService)
        {
            _rezervacijaService = rezervacijaService;
        }

        [HttpGet("moje")]
        public async Task<ActionResult<List<GetRezervacijaDto>>> GetForCurrentUser(Guid sobaId)
        {
            return Ok(await _rezervacijaService.GetForCurrentUser());
        }

    }
}

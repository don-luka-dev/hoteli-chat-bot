using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/Korisnici")]
    [ApiController]
    public class KorisnikController : Controller
    {
        private readonly IKorisnikService _korisnikService;

        public KorisnikController(IKorisnikService korisnikService)
        {
            _korisnikService = korisnikService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Korisnik>>> GetAll([FromQuery] KorisnikFilter filter)
        {

            var result = await _korisnikService.GetAll(filter);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GetKorisnikDto>> Delete(int id)
        {
            var result = await _korisnikService.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPut("{id:int}/promocija")]
        public async Task<ActionResult<GetKorisnikDto>> Promote(int id)
        {
            var result = await _korisnikService.Promote(id);
            if (!result) return NotFound();
            return Ok();
        }
    }
}

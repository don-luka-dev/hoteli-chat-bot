using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoteliController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        private readonly ILogger<HoteliController> logger;
        public HoteliController(IHotelService hotelService, ILogger<HoteliController> logger)
        {
            _hotelService = hotelService;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IList<GetHotelDto>>> GetAll([FromQuery] HotelFilterDto filter)
        {
            logger.LogDebug("Dohvaćanje svih hotela s filterima");
            return Ok(await _hotelService.GetAll(filter));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetHotelDto>> GetById(Guid id)
        {
            var hotelDto = await _hotelService.GetById(id);
            if (hotelDto == null) return NotFound();
            return Ok(hotelDto);
        }

        [HttpPost]
        public async Task<ActionResult<GetHotelDto?>> Add([FromForm] UpsertHotelDto upsertHotelDto)
        {
            try
            {
                var hotelDto = await _hotelService.Add(upsertHotelDto);
                return Ok(hotelDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<GetHotelDto>> Update(Guid id, [FromForm] UpsertHotelDto upsertHotelDto)
        {
            var hotelDto = await _hotelService.Update(id, upsertHotelDto);
            if (hotelDto == null) return NotFound();
            return Ok(hotelDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<GetHotelDto>> Delete(Guid id)
        {
            var result = await _hotelService.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpGet("statusi")]
        public async Task<ActionResult<List<IdValueDto>>> GetAllStatusiHotela()
        {
            var statusi = await _hotelService.GetAllStatusiHotela();
            return Ok(statusi);

        }

       
        // Dohvati sve hotele kojima je trenutni korisnik upravitelj.
        [HttpGet("moji")] // api/hoteli/moji
        public async Task<ActionResult<List<GetHotelDto>>> GetAllForUser()
        {
            var hotels = await _hotelService.GetAllForUser();

            if (hotels == null || hotels.Count == 0)
                return NotFound("Korisnik nema nijedan hotel.");

            return Ok(hotels);
        }

        [HttpGet("nepotvrdeni")]
        public async Task<ActionResult<List<GetHotelDto>>> GetUnconfiermed()
        {
            var hotels = await _hotelService.GetUnconfirmed();

            if (hotels == null || hotels.Count == 0)
                return NotFound("Nema novih hotela");

            return Ok(hotels);
        }

        [HttpPut("{id:guid}/potvrda")]
        public async Task<ActionResult<GetHotelDto>> Confirm(Guid id)
        {
            var result = await _hotelService.Confirm(id);
            if (!result) return NotFound();
            return Ok();
        }

    }

 
}

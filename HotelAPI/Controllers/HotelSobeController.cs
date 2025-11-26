using HotelAPI.Models.Dto;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/Hoteli/{hotelId:guid}/Sobe")]
    [ApiController]
    public class HotelSobeController : ControllerBase
    {
        private readonly IHotelSobaService sobaService;
        private readonly ILogger<HotelSobeController> logger;

        public HotelSobeController(IHotelSobaService sobaService, ILogger<HotelSobeController> logger)
        {
            this.sobaService = sobaService;
            this.logger = logger;
        }


        [HttpGet]
        public async Task<ActionResult<List<GetSobaDto>>> GetAllForHotel(Guid hotelId)
        {
            return Ok(await sobaService.GetAllForHotel(hotelId));
        }

        [HttpGet("{sobaId:guid}")]
        public async Task<ActionResult<GetSobaDto>> GetById(Guid hotelId, Guid sobaId)
        {
            var soba = await sobaService.GetById(hotelId, sobaId);
            if (soba == null) return NotFound();
            return Ok(soba);
        }

        [HttpPost]
        public async Task<ActionResult<GetSobaDto>> Add(Guid hotelId,[FromForm] UpsertSobaDto sobaDto)
        {
            try
            {
                var soba = await sobaService.Add(hotelId, sobaDto);
                return Ok(soba);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{sobaId:guid}")]
        public async Task<ActionResult<GetSobaDto>> Update(Guid hotelId, Guid sobaId, UpsertSobaDto sobaDto)
        {
            var soba = await sobaService.Update(hotelId, sobaId, sobaDto);
            if (soba == null) return NotFound();
            return Ok(soba);
        }

        [HttpDelete("{sobaId:guid}")]
        public async Task<IActionResult> Delete(Guid hotelId, Guid sobaId)
        {
            var result = await sobaService.Delete(hotelId, sobaId);
            if (!result) return NotFound();
            return Ok();
        }
    }
}


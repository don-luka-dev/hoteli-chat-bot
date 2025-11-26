// api/Sobe
using HotelAPI.Models.Dto;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/Sobe")]
[ApiController]
public class SobeController : ControllerBase
{
    private readonly ISobaService sobaService;
    public SobeController(ISobaService sobaService) => this.sobaService = sobaService;

    [HttpGet]
    public async Task<ActionResult<List<GetSobaDto>>> GetAll([FromQuery] SobaFilterDto filter)
        => Ok(await sobaService.GetAll(filter));


    [HttpGet("{sobaId:guid}")]
    public async Task<ActionResult<GetSobaDto>> GetById(Guid sobaId)
    {
        var soba = await sobaService.GetById(sobaId);
        if (soba == null) return NotFound();
        return Ok(soba);
    }

}

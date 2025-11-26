using HotelAPI.Models.Dto;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<GetKorisnikDto>> Register([FromBody] UpsertKorisnikDto korisnikDto)
        {
            var korisnik = await _authService.Register(korisnikDto);

            if (korisnik == null)
                return BadRequest(new { poruka = "Registracija nije uspjela." });

            return Ok(korisnik);
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto request)
        {
            var result = await _authService.Login(request);

            if (result == null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
                return Unauthorized(new { poruka = "Pogrešni podaci za prijavu." });

            return Ok(result);
        }

        [HttpPost("refresh-tokens")]
        public async Task<ActionResult<TokenResponseDto>> RefreshTokens([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokens(request);

            if (result == null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
                return Unauthorized(new { poruka = "Neispravni tokeni." });

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var rezultat = await _authService.Logout();

            if (rezultat)
                return Ok(new { Poruka = "Uspješna odjava." });

            return BadRequest(new { Poruka = "Greška pri odjavi." });
        }
    }

}

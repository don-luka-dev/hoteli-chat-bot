using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;

namespace HotelAPI.Service.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponseDto?> Login(LoginDto loginDto);
        Task<GetKorisnikDto> Register(Models.Dto.UpsertKorisnikDto korisnikDto);
        Task<TokenResponseDto?> RefreshTokens(RefreshTokenRequestDto request);
        Task<Korisnik?> GetCurrentUserAsync();
        Task<bool> Logout();

    }
}

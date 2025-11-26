using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HotelAPI.Service.Implementations
{
    public class AuthService(ApplicationDbContext dbContext,UserManager<Korisnik> userManager, 
        IServiceProvider serviceProvider, 
        IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IAuthService
    {

        private readonly UserManager<Korisnik> _userManager = userManager;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<TokenResponseDto?> Login(LoginDto loginDto)
        {
            var korisnik = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginDto.Username);
            if (korisnik == null)
                return null;

            var uloga = await _userManager.GetRolesAsync(korisnik);

            if (new PasswordHasher<Korisnik>().VerifyHashedPassword(korisnik, korisnik.PasswordHash, loginDto.Lozinka) //mmmm da
                == PasswordVerificationResult.Failed)
                return null;

            TokenResponseDto response = await CreateTokenResponse(korisnik, uloga);

            return response;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(Korisnik korisnik, IList<string> uloga)
        {
            return new TokenResponseDto
            {
                UserId = korisnik.Id,
                AccessToken = CreateToken(korisnik, uloga),
                RefreshToken = await GenerateAndSaveRefreshToken(korisnik)

            };
        }

        public async Task<GetKorisnikDto> Register(UpsertKorisnikDto korisnikDto)
        {
            var korisnik = korisnikDto.Adapt<Korisnik>();
            korisnik.Ime = korisnikDto.Ime;           
            korisnik.Prezime = korisnikDto.Prezime;   
            korisnik.UserName = korisnikDto.Username;
            korisnik.Email = korisnikDto.Email;
            korisnik.SecurityStamp = Guid.NewGuid().ToString();

            var passwordHasher = new PasswordHasher<Korisnik>();
            korisnik.PasswordHash = passwordHasher.HashPassword(korisnik, korisnikDto.Lozinka ?? string.Empty);

            await _dbContext.Users.AddAsync(korisnik);
            await _dbContext.SaveChangesAsync();

            var userManager = _serviceProvider.GetRequiredService<UserManager<Korisnik>>();

            //Pozvati konstantu gdje ti je definiran taj string "Registrirani korisnik", ako se slučajno mijenja taj šifrarnik da ti ne ostaje magic string.
            const string defaultRole = "Registrirani korisnik";

            var result = await _userManager.AddToRoleAsync(korisnik, defaultRole);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Dodavanje uloge nije uspjelo: {errors}");
            }

            return korisnik.Adapt<GetKorisnikDto>();
        }

        public async Task<bool> Logout()
        {
            var korisnik = await GetCurrentUserAsync();

            if (korisnik == null)
                return false;

            korisnik.RefreshToken = null;
            korisnik.RefreshTokenExpiration = null;

            await _dbContext.SaveChangesAsync();
            return true;
        }



        public async Task<TokenResponseDto?> RefreshTokens(RefreshTokenRequestDto request)
        {
            var korisnik = await ValidateRefreshToken(request.UserId, request.RefreshToken);

            if (korisnik is null)
                throw new Exception("Nevažeći refresh token ili korisnik ne postoji.");

            var uloga = await _userManager.GetRolesAsync(korisnik);

            return await CreateTokenResponse(korisnik,uloga);

        }

        private async Task<Korisnik?> ValidateRefreshToken(int userId, string refreshToken)
        {
            var korisnik = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId && u.RefreshToken == refreshToken && u.RefreshTokenExpiration >= DateTime.UtcNow);
            if (korisnik != null)
            {
                return korisnik;
            }

            return null;
        }   

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
            
        }

        private async Task<string> GenerateAndSaveRefreshToken(Korisnik korisnik)
        {
            var refreshToken = GenerateRefreshToken();
            korisnik.RefreshToken = refreshToken;
            korisnik.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }
       
        private string CreateToken(Korisnik korisnik,ICollection<string> uloga)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, korisnik.UserName),
                new(ClaimTypes.GivenName, korisnik.Ime + " " + korisnik.Prezime),
                new(ClaimTypes.NameIdentifier, korisnik.Id.ToString()),
                new(ClaimTypes.Role, uloga.FirstOrDefault() ?? ""),
                new(ClaimTypes.Email, korisnik.Email ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<Korisnik?> GetCurrentUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return null;
            }

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return null;

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return null;

            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}

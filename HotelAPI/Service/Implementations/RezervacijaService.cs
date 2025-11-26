using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Service.Implementations
{
    public class RezervacijaService(ApplicationDbContext dbContext, IAuthService authService) : IRezervacijaService
    {
        private readonly ApplicationDbContext dbContext = dbContext;
        private readonly IAuthService authService = authService;

        public async Task<GetRezervacijaDto?> GetByUserId(int userId)
        {
            var rez = await dbContext.Rezervacije
                .AsNoTracking()
                .Where(r => r.KorisnikId == userId)
                .OrderByDescending(r => r.DatumKreiranja) 
                .FirstOrDefaultAsync();

            return rez?.Adapt<GetRezervacijaDto>();
        }

       
        /// Vrati sve rezervacije za trenutno prijavljenog korisnika.
        public async Task<List<GetRezervacijaDto>> GetForCurrentUser()
        {
            var user = await authService.GetCurrentUserAsync();
            if (user is null) return new List<GetRezervacijaDto>();

            var rezervacije = await dbContext.Rezervacije
                .AsNoTracking()
                .Where(r => r.KorisnikId == user.Id)
                .OrderByDescending(r => r.DatumKreiranja)
                .ToListAsync();

            return rezervacije.Adapt<List<GetRezervacijaDto>>();
        }

        public async Task<bool> Delete(Guid id)
        {
            var rezervacja = await dbContext.Rezervacije.FindAsync(id);
            if (rezervacja is null) return false;

            dbContext.Rezervacije.Remove(rezervacja);
            await dbContext.SaveChangesAsync();
            return true;
        }

    }
}

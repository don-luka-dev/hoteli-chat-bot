using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Service.Implementations
{
    public class SobaRezervacijaService(ApplicationDbContext dbContext) : ISobaRezervacijaService

    {
        private readonly ApplicationDbContext dbContext = dbContext;

        public async Task<GetRezervacijaDto?> Add(Guid sobaId, UpsertRezervacijaDto rezervacijaDto)
        {
            var rezervacija = rezervacijaDto.Adapt<Rezervacija>();
            rezervacija.Id = Guid.NewGuid();
            rezervacija.DatumKreiranja = DateTime.UtcNow;          
            rezervacija.SobaId = sobaId;

            await dbContext.Rezervacije.AddAsync(rezervacija);
            await dbContext.SaveChangesAsync();

            return rezervacija.Adapt<GetRezervacijaDto>();
        }

        public async Task<bool> Delete(Guid sobaId, Guid rezervacijaId)
        {
            var rezervacja = await dbContext.Rezervacije.FindAsync(rezervacijaId);
            if (rezervacja is null) return false;

            dbContext.Rezervacije.Remove(rezervacja);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetRezervacijaDto>> GetAllForSoba(Guid sobaId)
        {
            var rezervacije = await dbContext.Rezervacije
                .Where(r => r.SobaId == sobaId)
                .AsNoTracking()
                .ToListAsync();

            return rezervacije.Adapt<List<GetRezervacijaDto>>();
        }

        public async Task<GetRezervacijaDto?> GetById(Guid sobaId, Guid rezervacijaId)
        {
            var rezervacija = await dbContext.Rezervacije
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == rezervacijaId);

            return rezervacija.Adapt<GetRezervacijaDto?>();
        }

        public async Task<GetRezervacijaDto?> Update(Guid sobaId, Guid rezervacijaId, UpsertRezervacijaDto rezervacijaDto)
        {
            var rezervacija = await dbContext.Rezervacije
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == rezervacijaId);

            if (rezervacija is null) return null;

            rezervacija.DatumOd = rezervacijaDto.DatumOd;
            rezervacija.DatumDo = rezervacijaDto.DatumDo;
            rezervacija.Napomena = rezervacijaDto.Napomena;
            rezervacija.KorisnikId = rezervacijaDto.KorisnikId;
            rezervacija.SobaId = rezervacijaDto.SobaId;

            await dbContext.SaveChangesAsync();

            return rezervacija.Adapt<GetRezervacijaDto>();

        }
    }
}

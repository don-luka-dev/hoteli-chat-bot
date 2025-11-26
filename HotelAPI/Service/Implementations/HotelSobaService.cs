using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Service.Implementations
{
    public class HotelSobaService(ApplicationDbContext dbContext, IValidator<UpsertSobaDto> validator, ISlikaSobeService slikaSobaService) : IHotelSobaService

    {
        private readonly ApplicationDbContext dbContext = dbContext;
        private readonly IValidator<UpsertSobaDto> validator = validator;
        private readonly ISlikaSobeService slikaSobaService = slikaSobaService;

        public async Task<GetSobaDto?> Add(Guid hotelId, UpsertSobaDto sobaDto)
        {
            var validationResult = await validator.ValidateAsync(sobaDto);
            if (!validationResult.IsValid) return null;

            var soba = sobaDto.Adapt<Soba>();
            soba.Id = Guid.NewGuid();
            soba.HotelId = hotelId;

            var hotel = await dbContext.Hoteli
                .Where(h => !h.IsDeleted)
                .FirstOrDefaultAsync(h => h.Id == sobaDto.HotelId);

            if (hotel is null) return null;

            soba.Hotel = hotel;

            await dbContext.Sobe.AddAsync(soba);
            await dbContext.SaveChangesAsync();

            if (sobaDto.NaslovnaSlika != null)
            {
                var slika = await slikaSobaService.Add(soba.Id, sobaDto.NaslovnaSlika);
                soba.UrlSlike = slika.UrlSlike;

                dbContext.Sobe.Update(soba);
                await dbContext.SaveChangesAsync();
            }

            return soba.Adapt<GetSobaDto>();
        }

        


        public async Task<List<GetSobaDto>> GetAllForHotel(Guid hotelId)
        {
            var sobe = await dbContext.Sobe
                .Where(s => s.HotelId == hotelId && !s.IsDeleted)
                .AsNoTracking()
                .Include(s => s.Hotel)
                .ToListAsync();

            return sobe.Adapt<List<GetSobaDto>>();
        }

        public async Task<GetSobaDto?> GetById(Guid hotelId, Guid sobaId)
        {
            var soba = await dbContext.Sobe
                .Where(s => s.HotelId == hotelId && !s.IsDeleted)
                .AsNoTracking()
                .Include(s => s.Hotel)
                .FirstOrDefaultAsync(s => s.Id == sobaId && s.HotelId == hotelId);

            if (soba is null) return null;

            return soba?.Adapt<GetSobaDto>();
        }


        //TODO nedovrseno
        public async Task<GetSobaDto?> Update(Guid hotelId, Guid sobaId, UpsertSobaDto sobaDto)
        {
            //TODO dovrsiti sa exepction handlerom(kad napravim execption handler)
            var validationResult = validator.ValidateAsync(sobaDto);
            if (validationResult.IsFaulted) return null;

            var soba = await dbContext.Sobe.FirstOrDefaultAsync(s => s.Id == sobaId && s.HotelId == hotelId);
            if (soba is null) return null;

            soba.Naziv = sobaDto.Naziv;
            soba.BrojKreveta = sobaDto.BrojKreveta;
            soba.CijenaNocenja = sobaDto.CijenaNocenja;
            soba.HotelId = sobaDto.HotelId;

            await dbContext.SaveChangesAsync();

            return soba.Adapt<GetSobaDto>();
        }

        public async Task<bool> Delete(Guid hotelId, Guid sobaId)
        {
            var soba = await dbContext.Sobe.FirstOrDefaultAsync(s => s.Id == sobaId && s.HotelId == hotelId);
            if (soba is null) return false;

            soba.IsDeleted = true;

            var rezervacije = await dbContext.Rezervacije
                  .Where(r => r.SobaId == soba.Id)
                  .ToListAsync();

            foreach (var r in rezervacije)
            {
                dbContext.Remove(r);
            }

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Soba?> GetByHotelIdAndNameAsync(Guid hotelId, string nazivSobe)
        {
            return await dbContext.Sobe.FirstOrDefaultAsync(s => s.HotelId == hotelId && s.Naziv == nazivSobe);
        }
    }
}


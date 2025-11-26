using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Service.Implementations
{
    public class SlikaSobeService(ApplicationDbContext dbContext) : ISlikaSobeService
    {
        private readonly ApplicationDbContext dbContext = dbContext;
        public async Task<List<GetSlikaSobeDto>> GetAllForSoba(Guid sobaId)
        {
            var slike = await dbContext.SlikeSoba
                .Where(s => s.SobaId == sobaId)
                .AsNoTracking()
                .ToListAsync();

            return slike.Adapt<List<GetSlikaSobeDto>>();
        }

        public async Task<GetSlikaSobeDto?> GetById(Guid sobaId, Guid slikaId)
        {
            var slika = await dbContext.SlikeSoba
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == slikaId && s.SobaId == sobaId);

            return slika?.Adapt<GetSlikaSobeDto>();
        }

        public async Task<GetSlikaSobeDto?> Add(Guid sobaId, UpsertSlikaSobeDto slikaDto)
        {
            slikaDto.SobaId = sobaId;
            var slika = slikaDto.Adapt<SlikaSobe>();
            slika.Id = Guid.NewGuid();

            if (slikaDto.UrlSlike is not null && slikaDto.UrlSlike.Length > 0)
            {
                slika.UrlSlike = await ConvertImageToBase64Async(slikaDto.UrlSlike);
            }
            else
            {
                throw new ArgumentException("Slika nije poslana.");
            }

            var soba = await dbContext.Sobe
                  .FirstOrDefaultAsync(s => s.Id == sobaId && s.UrlSlike == null);

            if (soba != null)
            {
                soba.UrlSlike = slika.UrlSlike;
                await dbContext.SaveChangesAsync();
            }

            slika.SobaId = sobaId;

            await dbContext.SlikeSoba.AddAsync(slika);
            await dbContext.SaveChangesAsync();

            return slika.Adapt<GetSlikaSobeDto>();
        }

        public async Task<GetSlikaSobeDto?> Update(Guid sobaId, Guid slikaId, UpsertSlikaSobeDto slikaDto)
        {
            var slika = await dbContext.SlikeSoba.FirstOrDefaultAsync(s => s.Id == slikaId && s.SobaId == sobaId);
            if (slika is null) return null;

            if (slikaDto.UrlSlike is not null && slikaDto.UrlSlike.Length > 0)
            {
                slika.UrlSlike = await ConvertImageToBase64Async(slikaDto.UrlSlike);
            }

            slika.NaslovSlike = slikaDto.NaslovSlike;
            slika.OpisSlike = slikaDto.OpisSlike;
            
            await dbContext.SaveChangesAsync();

            return slika.Adapt<GetSlikaSobeDto>();
        }

        public async Task<bool> Delete(Guid sobaId, Guid slikaId)
        {
            var slika = await dbContext.SlikeSoba.FirstOrDefaultAsync(s => s.Id == slikaId && s.SobaId == sobaId);
            if (slika is null) return false;

            dbContext.SlikeSoba.Remove(slika);
            await dbContext.SaveChangesAsync();
            return true;
        }

        private static async Task<string> ConvertImageToBase64Async(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var mimeType = file.ContentType;
            var base64 = Convert.ToBase64String(fileBytes);
            return $"data:{mimeType};base64,{base64}";
        }
    }
}

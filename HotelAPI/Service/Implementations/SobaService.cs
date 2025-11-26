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
    public class SobaService(ApplicationDbContext dbContext) : ISobaService
    {
        private readonly ApplicationDbContext dbContext = dbContext;
        
        public async Task<List<GetSobaDto>> GetAll(SobaFilterDto filter)
        {
            var query = dbContext.Sobe
                .Where(s => !s.IsDeleted)
                .Include(s => s.Hotel)
                .Include(s => s.Rezervacije)
                .AsQueryable();

            if (filter.BrojKreveta.HasValue)
                query = query.Where(s => s.BrojKreveta == filter.BrojKreveta);

            if (filter.HotelId != null)
                query = query.Where(s => s.HotelId == filter.HotelId);

            if (filter.CijenaNocenjaMin.HasValue && filter.CijenaNocenjaMax.HasValue)
            {
                var min = filter.CijenaNocenjaMin.Value;
                var max = filter.CijenaNocenjaMax.Value;
                if (min <= max)
                    query = query.Where(s => s.CijenaNocenja >= min && s.CijenaNocenja <= max);
                else
                    query = query.Where(_ => false); // mozda zamjeniti mjesta radije? -> ne seri
            }
            else if (filter.CijenaNocenjaMin.HasValue)
            {
                var min = filter.CijenaNocenjaMin.Value;
                query = query.Where(s => s.CijenaNocenja >= min);
            }
            else if (filter.CijenaNocenjaMax.HasValue)
            {
                var max = filter.CijenaNocenjaMax.Value;
                query = query.Where(s => s.CijenaNocenja <= max);
            }

            var result = await query
                .AsNoTracking()
                .ProjectToType<GetSobaDto>()
                .ToListAsync();

            return result;
        }

        public async Task<GetSobaDto?> GetById(Guid sobaId)
        {
            var soba = await dbContext.Sobe
                .AsNoTracking()
                .Include(s => s.Hotel)
                .FirstOrDefaultAsync(s => s.Id == sobaId);

            if (soba is null) return null;

            return soba?.Adapt<GetSobaDto>();
        }

    }
}

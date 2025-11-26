using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Service.Implementations
{
    public class MjestoService : IMjestoService
    {

        private readonly ApplicationDbContext dbContext;

        public MjestoService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GetMjestoDto>> GetAll()
        {
            var all = await dbContext.Mjesta.AsNoTracking().ToListAsync();

            return all.Adapt<List<GetMjestoDto>>();
        }

        public async Task<GetMjestoDto?> GetById(Guid id)
        {
            var mjesto = await dbContext.Mjesta.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id); 
            return mjesto.Adapt<GetMjestoDto>();
        }

        public async Task<GetMjestoDto> Add(UpsertMjestoDto mjestoDto)
        {
            var mjesto = mjestoDto.Adapt<Mjesto>();
            mjesto.Id = Guid.NewGuid();

            await dbContext.Mjesta.AddAsync(mjesto);
            await dbContext.SaveChangesAsync();

            return mjesto.Adapt<GetMjestoDto>();
        }



        public async Task<GetMjestoDto?> Update(Guid id, UpsertMjestoDto mjestoDto)
        {
            var mjesto = await dbContext.Mjesta.FindAsync(id);

            if (mjesto is null) return null;

            mjesto.Naziv = mjestoDto.Naziv;
            mjesto.PostanskiUredId = mjestoDto.PostanskiUredId;

            await dbContext.SaveChangesAsync();
            return mjesto.Adapt<GetMjestoDto>();

        }

        public async Task<bool> Delete(Guid id)
        {
            var mjesto = await dbContext.Mjesta.FindAsync(id);
            if (mjesto == null) return false;

            dbContext.Mjesta.Remove(mjesto);
            await dbContext.SaveChangesAsync();
            return true;
        }        
    }
}

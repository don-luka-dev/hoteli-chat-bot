using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace HotelAPI.Service.Implementations
{
    public class PostanskiUredService : IPostanskiUredService
    {

        private readonly ApplicationDbContext dbContext;

        public PostanskiUredService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<GetPostanskiUredDto>> GetAll()
        {
            var asd = await dbContext.PostanskiUredi.AsNoTracking().ToListAsync();

            return asd.Adapt<List<GetPostanskiUredDto>>();

        }

        public async Task<GetPostanskiUredDto?> GetById(Guid id)
        {
            var postanskiUred = await dbContext.PostanskiUredi.AsNoTracking().FirstOrDefaultAsync(pu => pu.Id == id);
            return postanskiUred.Adapt<GetPostanskiUredDto>();
        }

        public async Task<GetPostanskiUredDto> Add(UpsertPostanskiUredDto postanskiUredDto)
        {
            
            var postanskiUred = postanskiUredDto.Adapt<PostanskiUred>();
            postanskiUred.Id = Guid.NewGuid();

            await dbContext.PostanskiUredi.AddAsync(postanskiUred);
            await dbContext.SaveChangesAsync();

            return postanskiUred.Adapt<GetPostanskiUredDto>();
        }



        public async Task<GetPostanskiUredDto?> Update(Guid id, UpsertPostanskiUredDto postanskiUredDto)
        {
            var postanskiUred = await dbContext.PostanskiUredi.FindAsync(id);

            if (postanskiUred is null) return null;

            postanskiUred.Naziv = postanskiUredDto.Naziv;
            postanskiUred.PostanskiBroj = postanskiUredDto.PostanskiBroj;

            await dbContext.SaveChangesAsync();
           
            return postanskiUred.Adapt<GetPostanskiUredDto>();

        }

        public async Task<bool> Delete(Guid id)
        {
            var postanskiUred = await dbContext.PostanskiUredi.FindAsync(id);
            if (postanskiUred == null) return false;

            dbContext.PostanskiUredi.Remove(postanskiUred);
            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}

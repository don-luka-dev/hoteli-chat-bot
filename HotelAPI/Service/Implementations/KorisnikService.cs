using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Service.Implementations
{
    public class KorisnikService(ApplicationDbContext dbContext, UserManager<Korisnik> userManager) : IKorisnikService
    {
        private readonly ApplicationDbContext dbContext = dbContext;
        private readonly UserManager<Korisnik> _userManager = userManager;


        public Task<GetKorisnikDto?> Add([FromForm] UpsertKorisnikDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetKorisnikDto>> GetAll(KorisnikFilter? filter)
        {
            var query = dbContext.Users.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter?.KorisnickoIme))
            {
                var term = filter.KorisnickoIme.Trim();
                query = query.Where(k => k.UserName.Contains(term));
                // Or case-insensitive:
                // query = query.Where(k => EF.Functions.Like(k.UserName, $"%{term}%"));
            }

            // fetch users first
            var users = await query.ToListAsync();

            var result = new List<GetKorisnikDto>();

            foreach (var user in users)//magic stringovi
            {
                var dto = user.Adapt<GetKorisnikDto>();

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrator"))
                    dto.Rola = "Administrator";
                else if (roles.Contains("Upravitelj hotela"))
                    dto.Rola = "Upravitelj hotela";
                else
                    dto.Rola = roles.FirstOrDefault();

                result.Add(dto);
            }

            return result;
        }

        public Task<List<GetKorisnikDto>> GetAllAdmins()
        {
            throw new NotImplementedException();
        }

        public Task<List<GetKorisnikDto>> GetAllUpravitelji()
        {
            throw new NotImplementedException();
        }

        public Task<GetKorisnikDto?> GetById(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Delete(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null) return false;

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Promote(int id)
        {
            var user = await dbContext.Users.FindAsync(id);
            if (user == null) return false;
            await _userManager.AddToRoleAsync(user, "Administrator");//magic string

            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}

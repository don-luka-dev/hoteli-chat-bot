using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using HotelAPI.Service.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HotelAPI.Service.Implementations
{
    public class HotelService(ApplicationDbContext dbContext, IValidator<UpsertHotelDto> validator, IAuthService authService, UserManager<Korisnik> userManager) : IHotelService
    {

        private readonly ApplicationDbContext dbContext = dbContext;
        private readonly IValidator<UpsertHotelDto> validator = validator;
        private readonly IAuthService authService = authService;
        private readonly UserManager<Korisnik> _userManager = userManager;


        //add
        public async Task<GetHotelDto?> Add(UpsertHotelDto hotelDto)
        {
            var validationResult = await validator.ValidateAsync(hotelDto);
            if (!validationResult.IsValid) return null;

            var hotel = hotelDto.Adapt<Hotel>();
            hotel.Id = Guid.NewGuid();
            hotel.MjestoId = hotelDto.MjestoId;
            hotel.Mjesto = await dbContext.Mjesta.FindAsync(hotelDto.MjestoId);

            Korisnik? upravitelj = await authService.GetCurrentUserAsync();
            if (upravitelj is not null)
            {  
                hotel.Upravitelji.Add(upravitelj);

            }

            //popravit, maknut magic stringove
            var uloge = await _userManager.GetRolesAsync(upravitelj);
            var uloga = uloge.FirstOrDefault();
            Console.WriteLine(uloga.FirstOrDefault() + "userid");
            if (uloga == "Registrirani korisnik")
            {
                hotel.StatusHotelaId = 1;
                hotel.StatusHotela = await dbContext.StatusiHotela.FindAsync(1);
            }
            else
            {
                hotel.StatusHotelaId = hotelDto.StatusHotelaId;
                hotel.StatusHotela = await dbContext.StatusiHotela.FindAsync(hotelDto.StatusHotelaId);
            }


            if (hotelDto.UrlSlike is not null && hotelDto.UrlSlike.Length > 0)
            {
                hotel.UrlSlike = await ConvertImageToBase64Async(hotelDto.UrlSlike);
            }
            else if (hotelDto.AiUrlSlike is not null && hotelDto.AiUrlSlike.Length > 0)
            {
                hotel.UrlSlike = hotelDto.AiUrlSlike;
            }

            await dbContext.AddAsync(hotel);
            await dbContext.SaveChangesAsync();

            return hotel.Adapt<GetHotelDto>();
        }

        //delete
        public async Task<bool> Delete(Guid id)
        {
            var hotel = await dbContext.Hoteli.FindAsync(id);
            if (hotel == null) return false;

            hotel.IsDeleted = true;

            await dbContext.SaveChangesAsync();
            return true;
        }

        //get all
        public async Task<List<GetHotelDto>> GetAll(HotelFilterDto filter)
        {
            var query = dbContext.Hoteli
                .Where(h => !h.IsDeleted && h.StatusHotelaId == 2)//maknut magic number sad nam se zuri jbg
                .Include(h => h.Mjesto)
                .Include(h => h.StatusHotela)
                .AsQueryable();

            if (filter.MjestoId != null)
                query = query.Where(h => h.MjestoId == filter.MjestoId);

            if (filter.StatusHotelaId.HasValue)
                query = query.Where(h => h.StatusHotelaId == filter.StatusHotelaId);

            if (!string.IsNullOrWhiteSpace(filter.Naziv))
                query = query.Where(h =>
                    EF.Functions.Like(h.Naziv.ToLower(), $"%{filter.Naziv.ToLower()}%"));

            var result = await query
                .AsNoTracking()
                .ProjectToType<GetHotelDto>()
                .ToListAsync();

            return result;
        }


        //get by id
        public async Task<GetHotelDto?> GetById(Guid id)
        {
            var hotel = await dbContext.Hoteli
                .AsNoTracking()
                .Where(h => !h.IsDeleted) 
                .Include(h => h.Mjesto)
                .Include(h => h.StatusHotela)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel is null) return null;
            
            return hotel.Adapt<GetHotelDto>();

        }

        //update
        public async Task<GetHotelDto?> Update(Guid id, UpsertHotelDto hotelDto)
        {
            //TODO dovrsiti sa exepction handlerom(kad napravim execption handler)
            var validationResult = await validator.ValidateAsync(hotelDto);
            if (!validationResult.IsValid) return null;

            var hotel = await dbContext.Hoteli.FindAsync(id);
            if (hotel is null) return null;

            hotel.Naziv = hotelDto.Naziv;
            hotel.KontaktBroj = hotelDto.KontaktBroj;
            hotel.KontaktEmail = hotelDto.KontaktEmail;

            if (hotelDto.UrlSlike is not null && hotelDto.UrlSlike.Length > 0)
            {
                hotel.UrlSlike = await ConvertImageToBase64Async(hotelDto.UrlSlike);
            }

            if (hotel.MjestoId != hotelDto.MjestoId)
            {
                var mjesto = await dbContext.Mjesta.FindAsync(hotelDto.MjestoId) ?? throw new Exception("Mjesto ne postoji.");
                hotel.MjestoId = hotelDto.MjestoId;
                hotel.Mjesto = mjesto;
            }

            if (hotel.StatusHotelaId != hotelDto.StatusHotelaId)
            {
                var status = await dbContext.StatusiHotela.FindAsync(hotelDto.StatusHotelaId) ?? throw new Exception("Status hotela ne postoji.");
                hotel.StatusHotelaId = hotelDto.StatusHotelaId;
                hotel.StatusHotela = status;

            }

            await dbContext.SaveChangesAsync();

            return hotel.Adapt<GetHotelDto>(); 
        }

        //get za statuse
        public async Task<List<IdValueDto>> GetAllStatusiHotela()
        {
            var statusi = await dbContext.StatusiHotela.ToListAsync();
            return statusi.Adapt<List<IdValueDto>>();
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

        //grrr
        public async Task<Hotel?> GetByNameAsync(string nazivHotela)
        {
            return await dbContext.Hoteli.FirstOrDefaultAsync(h => h.Naziv == nazivHotela);
        }

        //get all for user
        public async Task<List<GetHotelDto>> GetAllForUser()
        {
            var user = await authService.GetCurrentUserAsync();
            if (user is null) return new List<GetHotelDto>(); // not null

            var hotels = await dbContext.Hoteli
               .AsNoTracking()
               .Where(h => !h.IsDeleted && h.Upravitelji.Any(u => u.Id == user.Id))
               .ProjectToType<GetHotelDto>()
               .ToListAsync();


            return hotels.Adapt<List<GetHotelDto>>();
        }

        public async Task<List<GetHotelDto>> GetUnconfirmed()
        {
            var hotels = await dbContext.Hoteli
              .AsNoTracking()
              .Where(h => !h.IsDeleted && h.StatusHotelaId == 1)//rijsit magic number
              .ProjectToType<GetHotelDto>()
              .ToListAsync();


            return hotels.Adapt<List<GetHotelDto>>();
        }

        public Task<List<GetHotelDto>> GetDenied()
        {
            throw new NotImplementedException();
        }

        //maknut magic stringove, lijenossttt jbg
        public async Task<bool> Confirm(Guid id)
        {
            var hotel = await dbContext.Hoteli.FindAsync(id);
            if (hotel == null) return false;

            hotel.StatusHotelaId = 2;
            hotel.StatusHotela = await dbContext.StatusiHotela.FindAsync(2);

            await dbContext.SaveChangesAsync();

            return true;
        }
    }
}

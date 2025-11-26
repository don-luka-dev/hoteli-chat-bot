using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Service.Interfaces
{
    public interface IHotelSobaService
    {
        Task<GetSobaDto?> Add(Guid hotelId, [FromForm] UpsertSobaDto sobaDto);
        Task<List<GetSobaDto>> GetAllForHotel(Guid hotelId);
        Task<GetSobaDto?> GetById(Guid hotelId, Guid sobaId);
        Task<GetSobaDto?> Update(Guid hotelId, Guid sobaId, [FromForm] UpsertSobaDto sobaDto);
        Task<bool> Delete(Guid hotelId, Guid sobaId);
        Task<Soba?> GetByHotelIdAndNameAsync(Guid hotelId, string nazivSobe);
    }
}

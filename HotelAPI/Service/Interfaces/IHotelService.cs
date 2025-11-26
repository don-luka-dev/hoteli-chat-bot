using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;
using HotelAPI.Models.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Service.Interfaces
{
    public interface IHotelService
    {
        Task<List<GetHotelDto>> GetAll(HotelFilterDto filter);
        Task<GetHotelDto?> GetById(Guid id);
        Task<GetHotelDto?> Add([FromForm] UpsertHotelDto hotelDto);
        Task<GetHotelDto?> Update(Guid id, [FromForm] UpsertHotelDto hotelDto);
        Task<bool> Delete(Guid id);
        Task<List<IdValueDto>> GetAllStatusiHotela();
        Task<Hotel?> GetByNameAsync(string nazivHotela);
        Task<List<GetHotelDto>> GetAllForUser();
        Task<List<GetHotelDto>> GetUnconfirmed();
        Task<bool> Confirm(Guid id);
        Task<List<GetHotelDto>> GetDenied();
    }
}

using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;

namespace HotelAPI.Service.Interfaces
{
    public interface IMjestoService
    {
        Task<List<GetMjestoDto>> GetAll();
        Task<GetMjestoDto?> GetById(Guid id);
        Task<GetMjestoDto> Add(UpsertMjestoDto mjestoDto);
        Task<GetMjestoDto?> Update(Guid id, UpsertMjestoDto mjestoDto);
        Task<bool> Delete(Guid id);
    }
}

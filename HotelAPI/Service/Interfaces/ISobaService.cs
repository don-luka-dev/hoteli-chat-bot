using HotelAPI.Models.Dto;
using HotelAPI.Models.Filters;

namespace HotelAPI.Service.Interfaces
{
    public interface ISobaService
    {
        Task<List<GetSobaDto>> GetAll(SobaFilterDto filter);
        Task<GetSobaDto?> GetById(Guid sobaId);

    }
}

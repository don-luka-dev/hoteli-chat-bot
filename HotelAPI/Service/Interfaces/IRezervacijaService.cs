using HotelAPI.Models.Dto;

namespace HotelAPI.Service.Interfaces
{
    public interface IRezervacijaService
    {
        Task<GetRezervacijaDto?> GetByUserId(int userId);
        Task<List<GetRezervacijaDto>> GetForCurrentUser();
        Task<bool> Delete(Guid id);

    }
}

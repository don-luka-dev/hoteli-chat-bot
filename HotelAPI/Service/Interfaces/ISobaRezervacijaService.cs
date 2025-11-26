using HotelAPI.Models.Dto;

namespace HotelAPI.Service.Interfaces
{
    public interface ISobaRezervacijaService
    {
        Task<GetRezervacijaDto?> Add(Guid sobaId, UpsertRezervacijaDto rezervacijaDto);
        Task<List<GetRezervacijaDto>> GetAllForSoba(Guid sobaId);
        Task<GetRezervacijaDto?> GetById(Guid sobaId, Guid rezervacijaId);
        Task<GetRezervacijaDto?> Update(Guid sobaId, Guid rezervacijaId, UpsertRezervacijaDto rezervacijaDto);
        Task<bool> Delete(Guid sobaId, Guid rezervacijaId);
    }
}

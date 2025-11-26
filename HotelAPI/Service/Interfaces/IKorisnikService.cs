using HotelAPI.Models.Dto;
using HotelAPI.Models.Filters;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Service.Interfaces
{
    public interface IKorisnikService
    {
        Task<List<GetKorisnikDto>> GetAll(KorisnikFilter filter);
        Task<GetKorisnikDto?> GetById(int id);
        Task<List<GetKorisnikDto>> GetAllAdmins();
        Task<List<GetKorisnikDto>> GetAllUpravitelji();
        Task<GetKorisnikDto?> Add([FromForm] UpsertKorisnikDto dto);
        Task<bool> Delete(int id);
        Task<bool> Promote(int id);
    }
}

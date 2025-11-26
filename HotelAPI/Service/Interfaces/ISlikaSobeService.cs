using HotelAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HotelAPI.Service.Interfaces
{
    public interface ISlikaSobeService
    {
        Task<List<GetSlikaSobeDto>> GetAllForSoba(Guid sobaId);
        Task<GetSlikaSobeDto?> GetById(Guid sobaId, Guid slikaId);
        Task<GetSlikaSobeDto?> Add(Guid sobaId,[FromForm] UpsertSlikaSobeDto dto);
        Task<GetSlikaSobeDto?> Update(Guid sobaId, Guid slikaId, UpsertSlikaSobeDto dto);
        Task<bool> Delete(Guid sobaId, Guid slikaId);
    }
}

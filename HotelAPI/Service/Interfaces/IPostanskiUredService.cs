using HotelAPI.Models.Dto;
using HotelAPI.Models.Entities;

namespace HotelAPI.Service.Interfaces
{
    public interface IPostanskiUredService
    {

        Task<List<GetPostanskiUredDto>> GetAll();
        Task<GetPostanskiUredDto?> GetById(Guid id);
        Task<GetPostanskiUredDto> Add(UpsertPostanskiUredDto postanskiUredDto);
        Task<GetPostanskiUredDto?> Update(Guid id, UpsertPostanskiUredDto postanskiUredDto);
        Task<bool> Delete(Guid id);

    }
}

using HotelAPI.Models.Entities;

namespace HotelAPI.Models.Dto
{
    public class GetKorisnikDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Rola { get; set; }

    }
}

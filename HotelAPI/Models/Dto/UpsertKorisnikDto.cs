namespace HotelAPI.Models.Dto
{
    public class UpsertKorisnikDto
    {
        public required string Username { get; set; }
        public string? Email { get; set; }
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public string? Lozinka { get; set; }
        public int? UlogaId { get; set; }
        public ICollection<Guid>? KorisnikoviHoteliIds { get; set; }

    }
}

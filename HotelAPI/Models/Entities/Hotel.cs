namespace HotelAPI.Models.Entities
{
    public class Hotel : Entity
    {
        public required string Naziv { get; set; }
        public required string KontaktBroj { get; set; }
        public required string KontaktEmail { get; set; }
        public required string Adresa { get; set; }
        public required string UrlSlike { get; set; }
        public Guid? MjestoId { get; set; }
        public Mjesto? Mjesto { get; set; }
        public int? StatusHotelaId { get; set; }
        public StatusHotela? StatusHotela { get; set; }
        public ICollection<Korisnik>? Upravitelji { get; set; } = new List<Korisnik>();
        public ICollection<Soba>? Sobe { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}

namespace HotelAPI.Models.Entities
{
    public class Rezervacija : Entity
    {
        public required DateTime DatumKreiranja { get; set; }
        public required DateTime DatumOd { get; set; }
        public required DateTime DatumDo { get; set; }
        public string? Napomena { get; set; }
        public required int KorisnikId { get; set; }
        public required Korisnik Korisnik { get; set; }
        public required Guid SobaId { get; set; }
        public required Soba Soba { get; set; }

    }
}

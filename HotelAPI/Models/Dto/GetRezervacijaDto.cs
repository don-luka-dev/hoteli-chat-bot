namespace HotelAPI.Models.Dto
{
    public class GetRezervacijaDto
    {
        public Guid Id { get; set; }
        public required DateTime DatumKreiranja { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string? Napomena { get; set; }
        public string? KorisnikIme { get; set; }
        public Guid SobaId { get; set; }
    }
}

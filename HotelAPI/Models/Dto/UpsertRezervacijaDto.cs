namespace HotelAPI.Models.Dto
{
    public class UpsertRezervacijaDto
    {
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public string? Napomena { get; set; }
        public int KorisnikId { get; set; }
        public Guid SobaId { get; set; }
    }
}

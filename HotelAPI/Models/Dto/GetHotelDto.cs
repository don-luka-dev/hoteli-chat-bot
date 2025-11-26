namespace HotelAPI.Models.Dto
{
    public class GetHotelDto
    {
        public required Guid Id { get; set; }
        public required string Naziv { get; set; }
        public required string KontaktBroj { get; set; }
        public required string KontaktEmail { get; set; }
        public required string Adresa { get; set; }
        public required string UrlSlike { get; set; }
        public Guid? MjestoId { get; set; }
        public string? MjestoNaziv { get; set; }
        public int? StatusHotelaId { get; set; }
        public string? StatusHotela { get; set; }
       
    }
}

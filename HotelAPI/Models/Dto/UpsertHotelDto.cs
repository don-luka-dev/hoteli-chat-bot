namespace HotelAPI.Models.Dto
{
    public class UpsertHotelDto
    {
        public required string Naziv { get; set; }
        public required string KontaktBroj { get; set; }
        public required string KontaktEmail { get; set; }
        public required string Adresa { get; set; }
        public IFormFile? UrlSlike { get; set; }
        public string? AiUrlSlike { get; set; }
        public Guid MjestoId { get; set; }
        public int StatusHotelaId { get; set; }
        public List<int>? UpraviteljIds { get; set; } = new List<int>();
        public List<Guid>? SobeIds { get; set; }

    }

}

namespace HotelAPI.Models.Dto
{
    public class GetMjestoDto
    {
        public Guid Id { get; set; }
        public required string Naziv { get; set; }
        public string? PostanskiUredNaziv { get; set; }
    }
}

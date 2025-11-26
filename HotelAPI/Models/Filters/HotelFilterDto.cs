namespace HotelAPI.Models.Filters
{
    public class HotelFilterDto
    {
        public Guid? MjestoId { get; set; }
        public int? StatusHotelaId { get; set; }
        public string? Naziv { get; set; }
    }
}

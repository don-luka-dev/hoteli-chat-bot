namespace HotelAPI.Models.Dto
{
    public class UpsertMjestoDto
    {
        public required string Naziv { get; set; }
        public Guid? PostanskiUredId { get; set; }

    }
}

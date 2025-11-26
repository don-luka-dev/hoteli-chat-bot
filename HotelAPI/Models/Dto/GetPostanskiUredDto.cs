namespace HotelAPI.Models.Dto
{
    public class GetPostanskiUredDto
    {
        public Guid Id { get; set; }
        public required string Naziv { get; set; }
        public required string PostanskiBroj { get; set; }
    }
}

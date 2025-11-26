namespace HotelAPI.Models.Entities
{
    public class SlikaSobe : Entity
    {

        public required string UrlSlike { get; set; }
        public string? NaslovSlike { get; set; }
        public string? OpisSlike { get; set; }
        public required Guid SobaId { get; set; }
        public Soba Soba { get; set; } = null!;

    }
}

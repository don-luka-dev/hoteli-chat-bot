namespace HotelAPI.Models.Entities
{
    public class Soba : Entity
    {
        public required string Naziv { get; set; }
        public int BrojKreveta { get; set; }
        public int CijenaNocenja { get; set; }
        public Guid HotelId { get; set; }
        public Hotel? Hotel { get; set; }
        public string? UrlSlike { get; set; }
        public ICollection<SlikaSobe>? SlikeSobe { get; set; }
        public ICollection<Rezervacija>? Rezervacije { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
    
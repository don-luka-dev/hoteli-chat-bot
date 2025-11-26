namespace HotelAPI.Models.Entities
{
    public class Mjesto : Entity
    {
        public required string Naziv { get; set; }
        public Guid? PostanskiUredId { get; set; }
        public virtual PostanskiUred? PostanskiUred { get; set; }
    }
}

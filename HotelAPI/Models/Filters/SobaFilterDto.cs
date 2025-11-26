namespace HotelAPI.Models.Filters
{
    public class SobaFilterDto
    {
        public int? BrojKreveta { get; set; }
        public int? CijenaNocenjaMin { get; set; }
        public int? CijenaNocenjaMax { get; set; }
        public Guid? HotelId { get; set; }
    }
}

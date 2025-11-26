namespace HotelAPI.Models.Dto
{
    public class UpsertSobaDto
    {
        public required string Naziv { get; set; }
        public int BrojKreveta { get; set; }
        public int CijenaNocenja { get; set; }
        public Guid HotelId { get; set; }
        public UpsertSlikaSobeDto? NaslovnaSlika { get; set; }
        public List<Guid>? SlikeSobeIds { get; set; }
    }
}

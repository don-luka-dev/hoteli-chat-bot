namespace HotelAPI.Models.Dto
{
    public class GetSobaDto
    {
        public Guid Id { get; set; }
        public required string Naziv { get; set; }
        public int BrojKreveta { get; set; }
        public int CijenaNocenja { get; set; }
        public string? UrlSlike { get; set; }
        public string? HotelNaziv { get; set; }
    }
}

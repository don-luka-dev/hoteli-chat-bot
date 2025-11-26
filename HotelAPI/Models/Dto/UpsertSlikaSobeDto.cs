namespace HotelAPI.Models.Dto
{
    public class UpsertSlikaSobeDto
    {
        //Ako je ovo File type, preimuneuj property iz UrlSlike u nešto smislenije(file, dokument...)
        public IFormFile? UrlSlike { get; set; }
        public string? NaslovSlike { get; set; }
        public string? OpisSlike { get; set; }
        public Guid? SobaId { get; set; }
    }
}

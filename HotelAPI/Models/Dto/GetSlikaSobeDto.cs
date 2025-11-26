namespace HotelAPI.Models.Dto
{
    public class GetSlikaSobeDto
    {
        public Guid Id { get; set; }
        //Pitanje: zašto je ovo nullable? Je li slika na sobi neobavezna ili?
        public string? UrlSlike { get; set; }
        public string? NaslovSlike { get; set; }
        public string? OpisSlike { get; set; }
        public required Guid SobaId { get; set; }
    }
}

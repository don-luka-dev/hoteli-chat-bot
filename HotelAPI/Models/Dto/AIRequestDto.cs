namespace HotelAPI.Models.Dto
{
    public class AIRequestDto
    {
        public string SessionId { get; set; } = default!;
        public string Message { get; set; } = default!;
        public IFormFile? UrlSlike { get; set; } = default!;

    }

}

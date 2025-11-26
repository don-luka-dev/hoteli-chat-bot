namespace HotelAPI.Models.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string SessionId { get; set; } = default!; 
        public string Role { get; set; } = default!; 
        public string Content { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string? UrlSlike { get; set; }
    }
}

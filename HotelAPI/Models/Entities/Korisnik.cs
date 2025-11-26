using Microsoft.AspNetCore.Identity;

namespace HotelAPI.Models.Entities
{
    public class Korisnik : IdentityUser<int>
    {

        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        public ICollection<Hotel>? KorisnikoviHoteli { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }

    }
}

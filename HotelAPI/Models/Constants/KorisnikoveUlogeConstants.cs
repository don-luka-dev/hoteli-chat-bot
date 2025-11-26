namespace HotelAPI.Models.Constants
{
    public static class KorisnikoveUlogeConstants
    {
        public const int SuperAdministrator = 1;
        public const int Administrator = 2;
        public const int UpraviteljHotela = 3;
        public const int RegistriraniKorisnik = 4;

        public static string Parse(int ulogaId) => ulogaId switch
        {
            SuperAdministrator => "Super administrator",
            Administrator => "Administrator",
            UpraviteljHotela => "Upravitelj hotela",
            RegistriraniKorisnik => "Registrirani korisnik",
            _ => throw new NotImplementedException(),
        };

        public static string ParseNormalized(int ulogaId) => ulogaId switch
        {
            SuperAdministrator => "SUPER ADMINISTRATOR",
            Administrator => "ADMINISTRATOR",
            UpraviteljHotela => "UPRAVITELJ HOTELA",
            RegistriraniKorisnik => "REGISTRIRANI KORISNIK",
            _ => throw new NotImplementedException(),
        };
    }

}

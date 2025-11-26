namespace HotelAPI.Models.Constants
{
    public class StatusHotelaConstants
    {

        public const int Nepotvrden = 1;
        public const int Aktivan = 2;
        public const int Neaktivan = 3;
        public const int Odbijen = 4;


        public static string Parse(int statusId) => statusId switch
        {
            Nepotvrden => nameof(Nepotvrden),
            Aktivan => nameof(Aktivan),
            Neaktivan => nameof(Neaktivan),
            Odbijen => nameof(Odbijen),
            _ => throw new NotImplementedException(),
        };
    }
}

namespace HotelAPI.Models.Constants
{
    public class StatusRezervacijeConstants
    {


        public const int UObradi = 1;
        public const int Prihvacena = 2;
        public const int Odbijena = 3;
        public const int Otkazana = 4;


        public static string Parse(int statusId) => statusId switch
        {
            UObradi => nameof(UObradi),
            Prihvacena => nameof(Prihvacena),
            Odbijena => nameof(Odbijena),
            Otkazana => nameof(Otkazana),
            _ => throw new NotImplementedException(),
        };
    }
}


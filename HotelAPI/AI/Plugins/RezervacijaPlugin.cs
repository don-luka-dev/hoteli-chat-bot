using HotelAPI.Models.Dto;
using HotelAPI.Service.Interfaces;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace HotelAPI.AI.Plugins
{
    public class RezervacijaPlugin(ISobaRezervacijaService rezervacijaService,
        IAuthService authService, IHotelService hotelService, IHotelSobaService sobaService)
    {
        private readonly IAuthService _authService = authService;
        private readonly IHotelService _hotelService = hotelService;
        private readonly IHotelSobaService _sobaService = sobaService;
        private readonly ISobaRezervacijaService _rezervacijaService = rezervacijaService;

        [KernelFunction("rezerviraj_sobu"), Description("Reserves a room based on user's specified hotel, room, and dates.")]
        public async Task<string> RezervirajSobuAsync(
           [Description("The name of the hotel where the user wants to reserve the room.")] string nazivHotela,
           [Description("The name or type of the room the user wishes to reserve.")] string nazivSobe,
           [Description("The start date of the reservation (check-in date).")] DateTime datumOd,
           [Description("The end date of the reservation (check-out date).")] DateTime datumDo,
           [Description("Additional notes or special requests from the user regarding the reservation.")] string? napomena)
        {
            napomena ??= "AI rezervacija";

            var korisnik = await _authService.GetCurrentUserAsync();
            if (korisnik == null) return "Molim ulogirajte se kako bi nastavili dalje.";

            var hotel = await _hotelService.GetByNameAsync(nazivHotela);
            if (hotel == null) return "Hotel nije pronađen.";

            var soba = await _sobaService.GetByHotelIdAndNameAsync(hotel.Id, nazivSobe);
            if (soba == null) return "Soba nije pronađena.";

            var upsertDto = new UpsertRezervacijaDto
            {
                DatumOd = datumOd,
                DatumDo = datumDo,
                Napomena = napomena,
                KorisnikId = korisnik!.Id,
                SobaId = soba.Id
            };

            var rezervacija = await _rezervacijaService.Add(soba.Id, upsertDto);

            if (rezervacija == null) return "Rezervacija nije uspješna";

            return $"Rezervirana soba '{soba.Naziv}' od {rezervacija.DatumOd:dd.MM.yyyy} do {rezervacija.DatumDo:dd.MM.yyyy} u hotelu {hotel.Naziv}.";
        }

    }

}

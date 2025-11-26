using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace HotelAPI.AI.Plugins
{
    public class SobaPlugin(IHotelSobaService sobaService, IAuthService authService, ApplicationDbContext dbContext)
    {
        private readonly IHotelSobaService _sobaService = sobaService;
        private readonly IAuthService _auth = authService;
        private readonly ApplicationDbContext _dbContext = dbContext;

        [KernelFunction("dodaj_sobu")]
        [Description("Adds a new room with the given name, number of beds, price per night, and the hotel it belongs to. Optionally accepts an image.")]
        public async Task<string> AddSobaAsync(
             [Description("Name of the room")] string naziv,
             [Description("Number of beds in the room")] int brojKreveta,
             [Description("Price of the room per night")] int cijenaNocenja,
             [Description("If asked what you need to add a hotel, for this variable say someting like a e.g. image of the hotel")] bool slikaBool,
             [Description("Name of the hotel this room belongs to")] string nazivHotela,
             Kernel kernel)
        {
            // izvuci sessionId koji je controller ubacio u Kernel.Data
            if (!kernel.Data.TryGetValue("sessionId", out var sidObj) || sidObj is not string sessionId)
                return "Nedostaje sessionId u kontekstu poziva.";

            var user = await _auth.GetCurrentUserAsync();
            if (user is null) return "Molim ulogirajte se kako biste dodali hotel.";

            var urlSlike = await _dbContext.ChatMessages
                .AsNoTracking()
                .Where(m => m.SessionId == sessionId && m.Role == "user" && !string.IsNullOrWhiteSpace(m.UrlSlike))
                .OrderByDescending(m => m.Timestamp)
                .Select(m => m.UrlSlike!)
                .FirstOrDefaultAsync();

            Guid hotelId = await _dbContext.Hoteli
                .Where(h => h.Naziv == nazivHotela)
                .Select(h => h.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(urlSlike))
                return "Za dodavanje hotela obavezno je priložiti sliku.";

            var dto = new UpsertSobaDto
            {
                Naziv = naziv,
                BrojKreveta = brojKreveta,
                CijenaNocenja = cijenaNocenja,
                HotelId = hotelId,

            };
            var created = await _sobaService.Add(hotelId, dto);
            return created is null
                ? "Dodavanje sobe nije uspjelo. Provjerite podatke."
                : $"Soba '{created.Naziv}' je dodan (ID: {created.Id}).";
        }
    }
}

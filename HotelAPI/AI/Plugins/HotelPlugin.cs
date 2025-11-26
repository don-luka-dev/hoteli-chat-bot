using HotelAPI.Data;
using HotelAPI.Models.Dto;
using HotelAPI.Service.Implementations;
using HotelAPI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using System.ComponentModel;

public class HotelPlugin(IHotelService hotelService, IAuthService authService, ApplicationDbContext dbContext, IMjestoService mjestoService)
{
    private readonly IHotelService _hotelService = hotelService;
    private readonly IAuthService _auth = authService;
    private readonly IMjestoService _mjestoService = mjestoService;
    private readonly ApplicationDbContext _dbContext = dbContext;

    [KernelFunction("dodaj_hotel")]
    [Description("Adds a new hotel with the given name, location (MjestoId),image , etc.")]
    public async Task<string> AddHotelAsync(
        [Description("Hotel name.")] string naziv,
        [Description("Contact phone number for the hotel.")] string kontaktBroj,
        [Description("Contact email for the hotel.")] string kontaktEmail,
        [Description("Street address of the hotel.")] string adresa,
        [Description("Location, city")] string mjesto,
        [Description("If asked what you need to add a hotel, for this variable say someting like a e.g. image of the hotel")] bool SlikaBool,
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

        if (string.IsNullOrWhiteSpace(urlSlike))
            return "Za dodavanje hotela obavezno je priložiti sliku.";

        Guid mjestoId = _dbContext.Mjesta
           .Where(m => m.Naziv == mjesto) 
           .Select(m => m.Id)
           .FirstOrDefault();

        var dto = new UpsertHotelDto 
        {
            Naziv = naziv,
            MjestoId = mjestoId,
            StatusHotelaId = 1,
            AiUrlSlike = urlSlike,
            KontaktBroj = kontaktBroj,
            KontaktEmail = kontaktEmail,
            Adresa = adresa,
            UpraviteljIds = [user.Id]
        };
        var created = await _hotelService.Add(dto);
        return created is null
            ? "Dodavanje hotela nije uspjelo. Provjerite podatke."
            : $"Hotel '{created.Naziv}' je dodan (ID: {created.Id}).";
    }
}

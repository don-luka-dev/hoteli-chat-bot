using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Validators
{
    public class HotelDtoValidator : AbstractValidator<UpsertHotelDto>
    {
        private readonly ApplicationDbContext dbContext;

        public HotelDtoValidator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            RuleFor(h => h.Naziv)
                .NotEmpty().WithMessage("Naziv hotela je obavezan");

            RuleFor(h => h.KontaktBroj)
                .NotEmpty().WithMessage("Kontakt broj je obavezan");

            RuleFor(h => h.KontaktEmail)
                .NotEmpty().WithMessage("Email je obavezan")
                .EmailAddress().WithMessage("Neispravan format email adrese");

            RuleFor(h => h.Adresa)
                .NotEmpty().WithMessage("Adresa hotela je obavezna");

            //RuleFor(h => h.UrlSlike)
            //    .NotEmpty().WithMessage("URL slike je obavezan")
            //    .Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute)).WithMessage("URL slike nije ispravan");

            RuleFor(h => h.MjestoId)
                .MustAsync(MjestoExists).WithMessage("Mjesto ne postoji");

            RuleFor(h => h.StatusHotelaId)
                .MustAsync(StatusHotelaExists).WithMessage("Status hotela ne postoji");

            RuleForEach(h => h.UpraviteljIds)
                .MustAsync(KorisnikExists).WithMessage("Jedan ili više upravitelja ne postoji");
        }

        private async Task<bool> MjestoExists(Guid id, CancellationToken ct)
        {
            return await dbContext.Mjesta.AnyAsync(m => m.Id == id, ct);
        }

        private async Task<bool> StatusHotelaExists(int id, CancellationToken ct)
        {
            return await dbContext.StatusiHotela.AnyAsync(s => s.Id == id, ct);
        }

        private async Task<bool> KorisnikExists(int id, CancellationToken ct)
        {
            return await dbContext.Users.AnyAsync(u => u.Id == id, ct);
        }
    }
}

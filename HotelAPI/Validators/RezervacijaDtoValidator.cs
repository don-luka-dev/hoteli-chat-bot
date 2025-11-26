using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Validators
{
    public class RezervacijaDtoValidator : AbstractValidator<UpsertRezervacijaDto>
    {
        private readonly ApplicationDbContext dbContext;

        public RezervacijaDtoValidator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            RuleFor(r => r.DatumOd)
                .LessThan(r => r.DatumDo)
                .WithMessage("Datum početka mora biti prije datuma završetka");

            RuleFor(r => r.DatumOd)
                .GreaterThan(DateTime.Now.AddDays(-1))
                .WithMessage("Datum početka mora biti u budućnosti");

            RuleFor(r => r.KorisnikId)
                .MustAsync(KorisnikExists).WithMessage("Korisnik ne postoji");

            RuleFor(r => r.SobaId)
                .MustAsync(SobaExists).WithMessage("Soba ne postoji");
        }

        private async Task<bool> KorisnikExists(int id, CancellationToken ct)
        {
            return await dbContext.Users.AnyAsync(k => k.Id == id, ct);
        }

        private async Task<bool> SobaExists(Guid id, CancellationToken ct)
        {
            return await dbContext.Sobe.AnyAsync(s => s.Id == id, ct);
        }
    }
}

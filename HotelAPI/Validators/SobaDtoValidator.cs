using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Validators
{
    public class SobaDtoValidator : AbstractValidator<UpsertSobaDto>
    {
        private readonly ApplicationDbContext dbContext;

        public SobaDtoValidator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            RuleFor(s => s.Naziv)
                .NotEmpty().WithMessage("Naziv sobe je obavezan");

            RuleFor(s => s.BrojKreveta)
                .GreaterThan(0).WithMessage("Broj kreveta mora biti veći od 0");

            RuleFor(s => s.CijenaNocenja)
                .GreaterThan(0).WithMessage("Cijena noćenja mora biti veća od 0");

            RuleFor(s => s.HotelId)
                .MustAsync(HotelExists).WithMessage("Hotel ne postoji");

            //RuleForEach(s => s.SlikeSobeIds ?? new List<Guid>())
            //    .MustAsync(SlikaSobeExists).WithMessage("Jedna ili više slika ne postoji");

        }

        private async Task<bool> HotelExists(Guid id, CancellationToken ct)
        {
            return await dbContext.Hoteli.AnyAsync(h => h.Id == id, ct);
        }

        private async Task<bool> SlikaSobeExists(Guid id, CancellationToken ct)
        {
            return await dbContext.SlikeSoba.AnyAsync(s => s.Id == id, ct);
        }

        private async Task<bool> RezervacijaExists(Guid id, CancellationToken ct)
        {
            return await dbContext.Rezervacije.AnyAsync(r => r.Id == id, ct);
        }
    }
}

using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Validators
{
    public class SlikaSobeDtoValidator : AbstractValidator<UpsertSlikaSobeDto>
    {
        private readonly ApplicationDbContext dbContext;

        public SlikaSobeDtoValidator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            //RuleFor(s => s.UrlSlike)
            //    .NotEmpty().WithMessage("URL slike je obavezan")
            //    .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            //    .WithMessage("URL nije ispravan");

            RuleFor(s => s.NaslovSlike)
                .NotEmpty().WithMessage("Naslov slike je obavezan");

            //RuleFor(s => s.SobaId)
            //    .MustAsync(SobaExists).WithMessage("Soba ne postoji");
        }

        private async Task<bool> SobaExists(Guid sobaId, CancellationToken ct)
        {
            return await dbContext.Sobe.AnyAsync(s => s.Id == sobaId, ct);
        }
    }
}

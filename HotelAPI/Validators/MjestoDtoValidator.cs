using FluentValidation;
using HotelAPI.Data;
using HotelAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI.Validators
{
    public class MjestoDtoValidator : AbstractValidator<UpsertMjestoDto>
    {
        private readonly ApplicationDbContext dbContext;
        public MjestoDtoValidator(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;

            RuleFor(m => m.Naziv)
                .NotNull().WithMessage("Naziv mjesta je potreban");

            RuleFor(m => m.PostanskiUredId)
                .MustAsync(PostanskiUredExists).WithMessage("Poštanski ured ne postoji");
                
        }

        private async Task<bool> PostanskiUredExists(Guid? id, CancellationToken ct)
        {
            var pu = await dbContext.PostanskiUredi.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
            if (pu != null) { return true; }
            return false;
        }
       
    }
}

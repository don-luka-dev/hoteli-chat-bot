using FluentValidation;
using HotelAPI.Models.Dto;

namespace HotelAPI.Validators
{
    public class PostanskiUredDtoValidator : AbstractValidator<UpsertPostanskiUredDto>
    {
        public PostanskiUredDtoValidator()
        {
            RuleFor(pu => pu.Naziv)
                .NotEmpty().WithMessage("Naziv je potreban");

            RuleFor(pu => pu.PostanskiBroj)
               .NotEmpty().WithMessage("Poštanski broj je potreban");

        }
    }
}

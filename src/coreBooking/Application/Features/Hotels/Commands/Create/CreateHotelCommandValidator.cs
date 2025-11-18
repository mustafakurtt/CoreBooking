using FluentValidation;
using Shared.Constants;

namespace Application.Features.Hotels.Commands.Create;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MaximumLength(EntityLengths.Name);

        RuleFor(c => c.City)
            .NotEmpty()
            .MaximumLength(EntityLengths.City);

        RuleFor(c => c.AddressStreet).NotEmpty().MaximumLength(EntityLengths.AddressLine);
        RuleFor(c => c.AddressCity).NotEmpty().MaximumLength(EntityLengths.City);
        RuleFor(c => c.AddressCountry).NotEmpty().MaximumLength(EntityLengths.City);
        RuleFor(c => c.AddressZipCode).NotEmpty().MaximumLength(EntityLengths.ZipCode);
    }
}
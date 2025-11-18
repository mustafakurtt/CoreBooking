using FluentValidation;
using Shared.Constants;

namespace Application.Features.Hotels.Commands.Update;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(EntityLengths.Name);
        RuleFor(c => c.City).NotEmpty().MaximumLength(EntityLengths.City);

        // Adres Parçalarý
        RuleFor(c => c.AddressStreet).NotEmpty().MaximumLength(EntityLengths.AddressLine);
        RuleFor(c => c.AddressCity).NotEmpty().MaximumLength(EntityLengths.City);
        RuleFor(c => c.AddressCountry).NotEmpty().MaximumLength(EntityLengths.City);
        RuleFor(c => c.AddressZipCode).NotEmpty().MaximumLength(EntityLengths.ZipCode);
    }
}
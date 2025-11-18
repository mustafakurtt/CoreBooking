using FluentValidation;

namespace Application.Features.Hotels.Commands.Update;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.City).NotEmpty();
        RuleFor(c => c.Address).NotEmpty();
    }
}
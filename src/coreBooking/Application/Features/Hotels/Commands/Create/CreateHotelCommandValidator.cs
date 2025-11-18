using FluentValidation;

namespace Application.Features.Hotels.Commands.Create;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.City).NotEmpty();
        RuleFor(c => c.Address).NotEmpty();
    }
}
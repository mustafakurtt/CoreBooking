using FluentValidation;

namespace Application.Features.Hotels.Commands.Create;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {

    }
}
using FluentValidation;

namespace Application.Features.Hotels.Commands.Delete;

public class DeleteHotelCommandValidator : AbstractValidator<DeleteHotelCommand>
{
    public DeleteHotelCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
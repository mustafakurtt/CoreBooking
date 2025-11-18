using FluentValidation;

namespace Application.Features.Bookings.Commands.Delete;

public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
{
    public DeleteBookingCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
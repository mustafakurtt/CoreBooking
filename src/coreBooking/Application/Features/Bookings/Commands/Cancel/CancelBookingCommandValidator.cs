using FluentValidation;

namespace Application.Features.Bookings.Commands.Cancel;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
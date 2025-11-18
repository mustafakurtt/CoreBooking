using FluentValidation;

namespace Application.Features.Bookings.Commands.Create;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(c => c.RoomTypeId).NotEmpty();
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.CheckInDate).NotEmpty();
        RuleFor(c => c.CheckOutDate).NotEmpty();
        RuleFor(c => c.NumberOfGuests).GreaterThan(0);
    }
}
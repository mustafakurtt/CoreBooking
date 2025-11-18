using FluentValidation;

namespace Application.Features.Bookings.Commands.Update;

public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.CustomerId).NotEmpty();
        RuleFor(c => c.RoomTypeId).NotEmpty();
        RuleFor(c => c.CheckInDate).NotEmpty();
        RuleFor(c => c.CheckOutDate).NotEmpty();
        RuleFor(c => c.NumberOfGuests).NotEmpty();
        RuleFor(c => c.TotalPrice).NotEmpty();
        RuleFor(c => c.Status).NotEmpty();
    }
}
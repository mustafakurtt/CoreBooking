using FluentValidation;

namespace Application.Features.RoomTypes.Commands.Update;

public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand>
{
    public UpdateRoomTypeCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.HotelId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Capacity).NotEmpty();
    }
}
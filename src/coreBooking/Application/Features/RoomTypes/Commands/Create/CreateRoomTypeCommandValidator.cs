using FluentValidation;

namespace Application.Features.RoomTypes.Commands.Create;

public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(c => c.HotelId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Capacity).NotEmpty();
    }
}
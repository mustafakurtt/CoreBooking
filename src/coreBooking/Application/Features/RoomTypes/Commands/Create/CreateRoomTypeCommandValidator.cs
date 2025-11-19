using FluentValidation;
using Shared.Constants;

namespace Application.Features.RoomTypes.Commands.Create;

public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(c => c.HotelId).NotEmpty();
        RuleFor(c => c.Name).NotEmpty().MaximumLength(EntityLengths.Name);
        RuleFor(c => c.Capacity).GreaterThan(0); // Kapasite en az 1 olmalý
    }
}
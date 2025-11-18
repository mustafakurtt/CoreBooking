using FluentValidation;

namespace Application.Features.RoomTypes.Commands.Delete;

public class DeleteRoomTypeCommandValidator : AbstractValidator<DeleteRoomTypeCommand>
{
    public DeleteRoomTypeCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
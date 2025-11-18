using FluentValidation;

namespace Application.Features.Inventories.Commands.Create;

public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(c => c.RoomTypeId).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();
        RuleFor(c => c.Quantity).NotEmpty();
    }
}
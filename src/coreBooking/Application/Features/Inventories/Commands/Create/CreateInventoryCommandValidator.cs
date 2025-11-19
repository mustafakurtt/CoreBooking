using Application.Features.Inventories.Constants;
using FluentValidation;

namespace Application.Features.Inventories.Commands.Create;

public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(c => c.RoomTypeId).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();

        RuleFor(c => c.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(InventoriesBusinessMessages.QuantityCannotBeNegative);

        RuleFor(c => c.PriceAmount)
            .GreaterThan(0)
            .WithMessage(InventoriesBusinessMessages.PriceMustBeGreaterThanZero);

        RuleFor(c => c.PriceCurrency).IsInEnum();
    }
}
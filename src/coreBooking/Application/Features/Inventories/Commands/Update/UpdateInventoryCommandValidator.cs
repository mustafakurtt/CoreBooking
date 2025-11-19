using Application.Features.Inventories.Constants;
using FluentValidation;

namespace Application.Features.Inventories.Commands.Update;

public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.RoomTypeId).NotEmpty();
        RuleFor(c => c.Date).NotEmpty();

        // Stok Validasyonu
        RuleFor(c => c.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(InventoriesBusinessMessages.QuantityCannotBeNegative);

        // Fiyat Validasyonu
        RuleFor(c => c.PriceAmount)
            .GreaterThan(0)
            .WithMessage(InventoriesBusinessMessages.PriceMustBeGreaterThanZero);

        RuleFor(c => c.PriceCurrency).IsInEnum();
    }
}
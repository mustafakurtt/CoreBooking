using FluentValidation;

namespace Application.Features.Inventories.Commands.Delete;

public class DeleteInventoryCommandValidator : AbstractValidator<DeleteInventoryCommand>
{
    public DeleteInventoryCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
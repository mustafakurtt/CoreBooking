using FluentValidation;

namespace Application.Features.Payments.Commands.Delete;

public class DeletePaymentCommandValidator : AbstractValidator<DeletePaymentCommand>
{
    public DeletePaymentCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
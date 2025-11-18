using FluentValidation;
using Shared.Constants;

namespace Application.Features.Payments.Commands.Update;

public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.BookingId).NotEmpty();

        // Para Validasyonu
        RuleFor(c => c.AmountValue).GreaterThan(0);
        RuleFor(c => c.AmountCurrency).IsInEnum();

        RuleFor(c => c.Date).NotEmpty();

        RuleFor(c => c.TransactionId)
            .NotEmpty()
            .MaximumLength(EntityLengths.TransactionId);

        RuleFor(c => c.Status).IsInEnum();
    }
}
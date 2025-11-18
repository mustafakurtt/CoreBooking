using FluentValidation;
using Shared.Constants;

namespace Application.Features.Payments.Commands.Create;

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();

        // Para Validasyonu
        RuleFor(c => c.AmountValue).GreaterThan(0); // 0 veya negatif olamaz
        RuleFor(c => c.AmountCurrency).IsInEnum();

        RuleFor(c => c.Date).NotEmpty();

        // TransactionId uzunluk kontrolü
        RuleFor(c => c.TransactionId)
            .NotEmpty()
            .MaximumLength(EntityLengths.TransactionId);

        RuleFor(c => c.Status).IsInEnum();
    }
}
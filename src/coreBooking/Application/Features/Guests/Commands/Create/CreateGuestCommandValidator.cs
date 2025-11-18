using FluentValidation;
using Shared.Constants;

namespace Application.Features.Guests.Commands.Create;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(EntityLengths.Name); // Veritabaný sýnýrý

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(EntityLengths.Name);

        RuleFor(c => c.Nationality)
            .NotEmpty()
            .MaximumLength(EntityLengths.Nationality); // Örn: 50 karakter

        // Mantýksal kontrol: Doðum tarihi gelecek bir tarih olamaz.
        RuleFor(c => c.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now).WithMessage("Doðum tarihi bugünden ileri olamaz.");

        // Not: IsPrimary boolean olduðu için NotEmpty() her zaman true döner (false da deðerdir). 
        // O yüzden ona kural yazmaya gerek yok.
    }
}
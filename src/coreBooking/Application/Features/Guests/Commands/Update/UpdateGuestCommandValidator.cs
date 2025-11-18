using FluentValidation;
using Shared.Constants;

namespace Application.Features.Guests.Commands.Update;

public class UpdateGuestCommandValidator : AbstractValidator<UpdateGuestCommand>
{
    public UpdateGuestCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.BookingId).NotEmpty();

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .MaximumLength(EntityLengths.Name);

        RuleFor(c => c.LastName)
            .NotEmpty()
            .MaximumLength(EntityLengths.Name);

        RuleFor(c => c.Nationality)
            .NotEmpty()
            .MaximumLength(EntityLengths.Nationality);

        RuleFor(c => c.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Now).WithMessage("Doðum tarihi bugünden ileri olamaz.");
    }
}
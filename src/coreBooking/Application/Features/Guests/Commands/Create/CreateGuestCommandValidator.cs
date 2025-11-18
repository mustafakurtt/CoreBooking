using FluentValidation;

namespace Application.Features.Guests.Commands.Create;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.DateOfBirth).NotEmpty();
        RuleFor(c => c.Nationality).NotEmpty();
        RuleFor(c => c.IsPrimary).NotEmpty();
    }
}
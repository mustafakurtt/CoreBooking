using FluentValidation;

namespace Application.Features.Guests.Commands.Update;

public class UpdateGuestCommandValidator : AbstractValidator<UpdateGuestCommand>
{
    public UpdateGuestCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.BookingId).NotEmpty();
        RuleFor(c => c.FirstName).NotEmpty();
        RuleFor(c => c.LastName).NotEmpty();
        RuleFor(c => c.DateOfBirth).NotEmpty();
        RuleFor(c => c.Nationality).NotEmpty();
        RuleFor(c => c.IsPrimary).NotEmpty();
    }
}
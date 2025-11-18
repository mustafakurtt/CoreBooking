using FluentValidation;

namespace Application.Features.Guests.Commands.Delete;

public class DeleteGuestCommandValidator : AbstractValidator<DeleteGuestCommand>
{
    public DeleteGuestCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
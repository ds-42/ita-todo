using FluentValidation;

namespace Users.Application.Features.User.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(t => t.User.Login).MinimumLength(4).MaximumLength(50).NotEmpty();
        RuleFor(t => t.User.Password).MinimumLength(4).MaximumLength(50).NotEmpty();
    }
}

using FluentValidation;

namespace Users.Application.Features.User.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(t => t.Login).MinimumLength(5).MaximumLength(50).NotEmpty();
        RuleFor(t => t.Password).MinimumLength(5).MaximumLength(50).NotEmpty();
    }
}

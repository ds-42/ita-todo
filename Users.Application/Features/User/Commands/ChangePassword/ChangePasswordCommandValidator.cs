using FluentValidation;
using Users.Application.Features.User.Commands.UpdateUser;

namespace Users.Application.Features.User.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    // faq: Хотелось бы получить общий валидатор
    public ChangePasswordCommandValidator()
    {
        RuleFor(t => t.Password).MinimumLength(4).MaximumLength(50).NotEmpty();
    }
}

using FluentValidation;
using Users.Services.Dto;

namespace Users.Services.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserDtoValidator() 
    {
        RuleFor(t => t.Login).MinimumLength(5).MaximumLength(50).NotEmpty();
        RuleFor(t => t.Password).MinimumLength(5).MaximumLength(50).NotEmpty();
    }
}

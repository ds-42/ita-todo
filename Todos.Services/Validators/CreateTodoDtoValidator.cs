using FluentValidation;
using Todos.Services.Dto;

namespace Todos.Services.Validators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoDto>
{
    public CreateTodoDtoValidator() 
    {
        RuleFor(t => t.OwnerId).GreaterThan(0).WithMessage("Invalid owner Id");
        RuleFor(t => t.Label).MinimumLength(5).MaximumLength(100);
    }
}

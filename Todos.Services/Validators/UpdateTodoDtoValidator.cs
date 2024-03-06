using FluentValidation;
using Todos.Services.Dto;

namespace Todos.Services.Validators;

public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
{
    public UpdateTodoDtoValidator()
    {
        RuleFor(t => t.OwnerId).GreaterThan(0).WithMessage("Invalid owner Id");
        RuleFor(t => t.Label).MinimumLength(5).MaximumLength(100);
    }
}

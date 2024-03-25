using FluentValidation;

namespace Todos.Application.Features.Todo.Commands.CreateTodo;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(t => t.OwnerId).GreaterThan(0).WithMessage("Invalid owner Id");
        RuleFor(t => t.Label).MinimumLength(5).MaximumLength(100);
    }
}

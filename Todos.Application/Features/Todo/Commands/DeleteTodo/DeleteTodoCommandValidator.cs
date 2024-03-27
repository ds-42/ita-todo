using FluentValidation;

namespace Todos.Application.Features.Todo.Commands.DeleteTodo;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleFor(t => t.Id)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Invalid todo Id");
    }
}

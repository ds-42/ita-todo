using FluentValidation;
using Todos.Application.Extensions;

namespace Todos.Application.Features.Todo.Commands.DeleteTodo;

public class DeleteTodoCommandValidator : AbstractValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleFor(t => t.Id).AsTodoId();
    }
}

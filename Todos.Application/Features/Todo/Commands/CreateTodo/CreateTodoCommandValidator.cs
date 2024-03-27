using FluentValidation;
using Todos.Application.Extensions;

namespace Todos.Application.Features.Todo.Commands.CreateTodo;

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(t => t.OwnerId).AsTodoOwnerId();
        RuleFor(t => t.Label).AsTodoLabel();
    }
}

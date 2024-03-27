using FluentValidation;
using Todos.Application.Extensions;

namespace Todos.Application.Features.Todo.Commands.UpdateTodo;

public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleFor(t => t.Id).AsTodoId();
        RuleFor(t => t.OwnerId).AsTodoOwnerId();
        RuleFor(t => t.Label).AsTodoLabel();
    }
}

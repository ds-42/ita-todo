using FluentValidation;
using Todos.Application.Extensions;

namespace Todos.Application.Features.Todo.Commands.DoneTodo;

public class DoneTodoCommandValidator : AbstractValidator<DoneTodoCommand>
{
    public DoneTodoCommandValidator()
    {
        RuleFor(t => t.Id).AsTodoId();
    }
}

namespace Todos.Application.Features.Todo.Commands.CreateTodo;

public class CreateTodoCommandValidator : TodoValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleForOwnerId(t => t.OwnerId);
        RuleForLabel(t => t.Label);
    }
}

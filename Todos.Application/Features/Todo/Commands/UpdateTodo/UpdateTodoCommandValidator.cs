namespace Todos.Application.Features.Todo.Commands.UpdateTodo;

public class UpdateTodoCommandValidator : TodoValidator<UpdateTodoCommand>
{
    public UpdateTodoCommandValidator()
    {
        RuleForId(t => t.Id);
        RuleForOwnerId(t => t.OwnerId);
        RuleForLabel(t => t.Label);
    }
}

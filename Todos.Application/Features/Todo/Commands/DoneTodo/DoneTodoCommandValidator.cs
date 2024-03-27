namespace Todos.Application.Features.Todo.Commands.DoneTodo;

public class DoneTodoCommandValidator : TodoValidator<DoneTodoCommand>
{
    public DoneTodoCommandValidator()
    {
        RuleForId(t => t.Id);
    }
}

namespace Todos.Application.Features.Todo.Commands.DeleteTodo;

public class DeleteTodoCommandValidator : TodoValidator<DeleteTodoCommand>
{
    public DeleteTodoCommandValidator()
    {
        RuleForId(t => t.Id);
    }
}

using MediatR;

namespace Todos.Application.Features.Todo.Commands.DeleteTodo;

public class DeleteTodoCommand : IRequest<bool>
{
    public int Id { get; set; }
}




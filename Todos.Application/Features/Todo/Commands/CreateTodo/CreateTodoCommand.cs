using MediatR;
using Todos.Application.Dto;

namespace Todos.Application.Features.Todo.Commands.CreateTodo;

public class CreateTodoCommand : IRequest<GetTodoDto>
{
    public int OwnerId { get; set; }
    public string Label { get; set; } = default!;
    public bool IsDone { get; set; }
}




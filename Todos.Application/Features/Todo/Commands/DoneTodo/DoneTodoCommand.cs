using MediatR;
using Todos.Application.Dto;

namespace Todos.Application.Features.Todo.Commands.DoneTodo;

public class DoneTodoCommand : IRequest<GetTodoDto>
{
    public int Id { get; set; }
}




using MediatR;
using Todos.Application.Dto;

namespace Todos.Application.Features.Todo.Commands.UpdateTodo;

public class UpdateTodoCommand : SetTodoDto, IRequest<GetTodoDto>
{
    public int Id { get; set; }
}




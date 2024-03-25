using MediatR;
using Todos.Application.Dto;

namespace Todos.Application.Features.Todo.Queries.GetById;

public class GetByIdQuery : IRequest<GetTodoDto>
{
    public int Id { get; set; }

    public GetByIdQuery(int id)
    {
        Id = id;
    }
}

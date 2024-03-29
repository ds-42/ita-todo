using Common.Application.Dto;
using MediatR;
using Todos.Application.Dto;

namespace Todos.Application.Features.Todo.Queries.GetList;

public class GetListQuery : Pagination, IRequest<CountableList<GetTodoDto>>
{
    public int? OwnerId { get; set; }
}

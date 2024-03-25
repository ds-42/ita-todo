using Common.Application.Dto;
using MediatR;

namespace Todos.Application.Features.Todo.Queries.GetCount;

public class GetCountQuery : BaseFilter, IRequest<int>
{
    public int? OwnerId { get; set; }
}

using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using Todos.Application.Dto;
using TodoModel = Common.Domain.Todo;

namespace Todos.Application.Features.Todo.Queries.GetById;

public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, GetTodoDto>
{
    private readonly IRepository<TodoModel> _todos;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(
        IRepository<TodoModel> todos,
        TodosMemoryCache cache,
        ICurrentUserService currentUser,
        IMapper mapper) : base(cache.Cache, 3)
    {
        _todos = todos;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public override async Task<GetTodoDto> ExecQuery(GetByIdQuery query, CancellationToken cancellationToken)
    {
        var item = await _todos.SingleAsync(t => t.Id == query.Id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        return _mapper.Map<GetTodoDto>(item);
    }
}

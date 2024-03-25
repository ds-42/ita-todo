using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Todos.Application.Dto;
using Common.Application.Abstractions;

namespace Todos.Application.Features.Todo.Queries.GetList;

public class GetListQueryHandler : IQueryHandler<GetListQuery, IReadOnlyCollection<GetTodoDto>>
{
    private readonly IRepository<Common.Domain.Todo> _todos;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetListQueryHandler(
        IRepository<Common.Domain.Todo> todos,
        TodosMemoryCache cache,
        ICurrentUserService currentUser,
        IMapper mapper) : base(cache.Cache, 1)
    {
        _todos = todos;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public override async Task<IReadOnlyCollection<GetTodoDto>> ExecQuery(GetListQuery query, CancellationToken cancellationToken)
    {
        var ownerId = query.OwnerId;
        if (!_currentUser.IsAdmin)
            ownerId = _currentUser.UserId;

        return _mapper.Map<IReadOnlyCollection<GetTodoDto>>(await _todos.GetItemsAsync(
            offset: query.Offset,
            limit: query.Limit,
            predicate: (query.Predicate == null && ownerId == null)? null : t => 
                (string.IsNullOrWhiteSpace(query.Predicate) ? true : t.Label.Contains(query.Predicate)) && 
                (ownerId == null ? true : t.OwnerId == ownerId),
            orderBy: t => t.Id, 
            destinct: null, 
            cancellationToken));
    }
}

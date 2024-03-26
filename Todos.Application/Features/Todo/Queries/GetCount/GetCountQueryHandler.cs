using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;

namespace Todos.Application.Features.Todo.Queries.GetCount;

public class GetCountQueryHandler : IQueryHandler<GetCountQuery, int>
{
    private readonly IRepository<Common.Domain.Todo> _todos;
    private readonly ICurrentUserService _currentUser;

    public GetCountQueryHandler(
        IRepository<Common.Domain.Todo> todos,
        ICurrentUserService currentUser,
        TodosMemoryCache cache) : base(cache.Cache, 1)
    {
        _currentUser = currentUser;
        _todos = todos;
    }

    protected override string GetCacheKey(GetCountQuery query)
    {
        return $"uid:{_currentUser.UserId}:{base.GetCacheKey(query)}";
    }

    public override async Task<int> ExecQuery(GetCountQuery query, CancellationToken cancellationToken)
    {
        var ownerId = query.OwnerId;
        if (!_currentUser.IsAdmin)
            ownerId = _currentUser.UserId;

        return await _todos.CountAsync(
            (query.Predicate == null && ownerId == null) ? null : t =>
                (string.IsNullOrWhiteSpace(query.Predicate) ? true : t.Label.Contains(query.Predicate)) &&
                (ownerId == null ? true : t.OwnerId == ownerId), 
            cancellationToken: cancellationToken);
    }
}

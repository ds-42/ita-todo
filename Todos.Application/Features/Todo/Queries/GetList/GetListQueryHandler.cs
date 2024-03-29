using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Todos.Application.Dto;
using Common.Application.Abstractions;
using System.Linq.Expressions;

using TodoModel = Common.Domain.Todo;
using Common.Application.Dto;

namespace Todos.Application.Features.Todo.Queries.GetList;

public class GetListQueryHandler : IQueryHandler<GetListQuery, CountableList<GetTodoDto>>
{
    private readonly IRepository<TodoModel> _todos;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public GetListQueryHandler(
        IRepository<TodoModel> todos,
        TodosMemoryCache cache,
        ICurrentUserService currentUser,
        IMapper mapper) : base(cache.Cache, 1)
    {
        _todos = todos;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    protected override string GetCacheKey(GetListQuery query)
    {
        return $"uid:{_currentUser.UserId}:{base.GetCacheKey(query)}";
    }

    public override async Task<CountableList<GetTodoDto>> ExecQuery(GetListQuery query, CancellationToken cancellationToken)
    {
        var ownerId = query.OwnerId;
        if (!_currentUser.IsAdmin)
            ownerId = _currentUser.UserId;

        Expression<Func<TodoModel, bool>> where = t => 
            (_currentUser.IsAdmin || t.OwnerId == _currentUser.UserId) &&
            (string.IsNullOrWhiteSpace(query.Predicate) ? true : t.Label.Contains(query.Predicate)) &&
            (ownerId == null ? true : t.OwnerId == ownerId);

        var items = _mapper.Map<IReadOnlyCollection<GetTodoDto>> (await _todos.GetItemsAsync(
            offset: query.Offset,
            limit: query.Limit,
            predicate: where,
            orderBy: t => t.Id, 
            destinct: null, 
            cancellationToken));

        return new CountableList<GetTodoDto>()
        {
            Count = await _todos.CountAsync(where, cancellationToken),
            Items = items,
        };
    }
}

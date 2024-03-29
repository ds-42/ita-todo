using AutoMapper;
using Users.Services.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Dto;
using Common.Domain.Users;
using System.Linq.Expressions;

namespace Users.Application.Features.User.Queries.GetList;

public class GetListQueryHandler : IQueryHandler<GetListQuery, CountableList<GetUserDto>>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly IMapper _mapper;

    public GetListQueryHandler(
        IRepository<ApplicationUser> userRepositiry,
        UsersMemoryCache cache,
        IMapper mapper) : base(cache.Cache, 1)
    {
        _users = userRepositiry;
        _mapper = mapper;
    }

    public override async Task<CountableList<GetUserDto>> ExecQuery(GetListQuery query, CancellationToken cancellationToken)
    {
        Expression<Func<ApplicationUser, bool>> where = t =>
            (string.IsNullOrWhiteSpace(query.Predicate) ? true : t.Login.Contains(query.Predicate));


        var items = _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _users.GetItemsAsync(
            query.Offset,
            query.Limit,
            where,
            t => t.Id, null, cancellationToken));

        return new CountableList<GetUserDto>
        {
            Count = await _users.CountAsync(where, cancellationToken: cancellationToken),
            Items = items,
        };
    }
}

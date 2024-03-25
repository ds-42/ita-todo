using Common.Application.Abstractions.Persistence;
using Common.Domain;

namespace Users.Application.Features.User.Queries.GetCount;

public class GetCountQueryHandler : IQueryHandler<GetCountQuery, int>
{
    private readonly IRepository<ApplicationUser> _users;

    public GetCountQueryHandler(
        IRepository<ApplicationUser> userRepositiry, UsersMemoryCache cache) : base(cache.Cache, 1)
    {
        _users = userRepositiry;
    }

    public override async Task<int> ExecQuery(GetCountQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.nameText))
        {
            return await _users.CountAsync(cancellationToken: cancellationToken);
        }
        else
        {
            return await _users.CountAsync(t => t.Login.Contains(query.nameText), cancellationToken: cancellationToken);
        }
    }
}

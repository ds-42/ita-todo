using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Users.Application;
using Users.Services.Dto;

namespace Users.Application.Query.GetCount;

public class GetCountQueryHandler : IRequestHandler<GetCountQuery, int>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly MemoryCache _cache;

    public GetCountQueryHandler(
        IRepository<ApplicationUser> userRepositiry, UsersMemoryCache cache)
    {
        _users = userRepositiry;
        _cache = cache.Cache;
    }

    public async Task<int> Handle(GetCountQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = $"Count:{query.JsonSerialize()}";

        if (_cache.TryGetValue(cacheKey, out int? result))
        {
            return result!.Value;
        }

        if (string.IsNullOrWhiteSpace(query.nameText))
        {
            result = await _users.CountAsync(cancellationToken: cancellationToken);
        }
        else
        {
            result = await _users.CountAsync(t => t.Login.Contains(query.nameText), cancellationToken: cancellationToken);
        }


        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
            .SetSlidingExpiration(TimeSpan.FromSeconds(5))
            .SetSize(1);

        _cache.Set(cacheKey, result, cacheEntryOptions);

        return result!.Value;

    }
}

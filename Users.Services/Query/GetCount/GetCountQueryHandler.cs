using Common.BL.Extensions;
using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Dto;

namespace Users.Services.Query.GetCount;

public class GetCountQueryHandler
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly MemoryCache _cache;

    public GetCountQueryHandler(
        IRepository<ApplicationUser> userRepositiry, UsersMemoryCache cache)
    {
        _users = userRepositiry;
        _cache = cache.Cache;
    }


    public async Task<int> RunAsync(BaseUsersFilter query, CancellationToken cancellationToken) 
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

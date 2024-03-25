using Common.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Common.Application.Abstractions.Persistence;

public abstract class ICacheQueryHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly MemoryCache _cache;
    private readonly int _cacheSize;

    public ICacheQueryHandler(MemoryCache cache, int cacheSize)
    {
        _cache = cache;
        _cacheSize = cacheSize;
    }

    protected virtual string GetCacheKey(TRequest query)
    {
        return $"{query.GetType().Name}:{query.JsonSerialize()}";
    }

    public async Task<TResponse> Handle(TRequest query, CancellationToken cancellationToken)
    {
        var cacheKey = GetCacheKey(query);

        if (_cache.TryGetValue(cacheKey, out TResponse? result))
        {
            return result!;
        }

        result = await ExecQuery(query, cancellationToken);

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
            .SetSlidingExpiration(TimeSpan.FromSeconds(5))
            .SetSize(_cacheSize);

        _cache.Set(cacheKey, result, cacheEntryOptions);

        return result;
    }

    public abstract Task<TResponse> ExecQuery(TRequest query, CancellationToken cancellationToken); 

}

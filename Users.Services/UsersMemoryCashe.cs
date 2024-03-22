using Microsoft.Extensions.Caching.Memory;

namespace Users.Services;

public class UsersMemoryCache
{
    public MemoryCache Cache { get; } = new MemoryCache(
        new MemoryCacheOptions
        {
            SizeLimit = 1024,
            ExpirationScanFrequency = TimeSpan.FromSeconds(1)
        });
}

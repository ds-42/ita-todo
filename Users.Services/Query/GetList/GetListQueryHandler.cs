using AutoMapper;
using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Users.Services.Dto;

namespace Users.Services.Query.GetList;

public class GetListQueryHandler
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly MemoryCache _memoryCache;

    public GetListQueryHandler(
        IRepository<ApplicationUser> userRepositiry,
        UsersMemoryCache cache,
        IMapper mapper)
    {
        _userRepository = userRepositiry;
        _mapper = mapper;
        _memoryCache = cache.Cache;
    }


    public async Task<IReadOnlyCollection<GetUserDto>> RunAsync(GetListQuery query, CancellationToken cancellationToken = default)
    {
        var cacheKey = Utils.SerializeObject(query);

        if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyCollection<GetUserDto>? result))
        {
            return result!;
        }

        result = _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _userRepository.GetItemsAsync(
            query.offset,
            query.limit,
            string.IsNullOrWhiteSpace(query.nameText)
                ? null
                : t => t.Login.Contains(query.nameText, StringComparison.InvariantCultureIgnoreCase),
            t => t.Id, null, cancellationToken));

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(30));

        _memoryCache.Set(cacheKey, result, cacheEntryOptions);

        return result;
    }

}

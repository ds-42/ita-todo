using AutoMapper;
using Common.Domain;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Users.Services.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using MediatR;

namespace Users.Application.Query.GetList;

public class GetListQueryHandler : IRequestHandler<GetListQuery, IReadOnlyCollection<GetUserDto>>
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly MemoryCache _cache;

    public GetListQueryHandler(
        IRepository<ApplicationUser> userRepositiry,
        UsersMemoryCache cache,
        IMapper mapper)
    {
        _userRepository = userRepositiry;
        _mapper = mapper;
        _cache = cache.Cache;
    }

    public async Task<IReadOnlyCollection<GetUserDto>> Handle(GetListQuery query, CancellationToken cancellationToken)
    {
        var cacheKey = query.JsonSerialize();

        if (_cache.TryGetValue(cacheKey, out IReadOnlyCollection<GetUserDto>? result))
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
            .SetAbsoluteExpiration(TimeSpan.FromSeconds(10))
            .SetSlidingExpiration(TimeSpan.FromSeconds(5))
            .SetSize(3);

        _cache.Set(cacheKey, result, cacheEntryOptions);

        return result;
    }

}

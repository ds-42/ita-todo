using AutoMapper;
using Common.Domain;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Users.Application.Features.User.Queries.GetList;

public class GetByIdQueryHandler : ICacheQueryHandler<GetListQuery, IReadOnlyCollection<GetUserDto>>
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(
        IRepository<ApplicationUser> userRepositiry,
        UsersMemoryCache cache,
        IMapper mapper) : base(cache.Cache, 1)
    {
        _userRepository = userRepositiry;
        _mapper = mapper;
    }

    public override async Task<IReadOnlyCollection<GetUserDto>> ExecQuery(GetListQuery query, CancellationToken cancellationToken)
    {
        return _mapper.Map<IReadOnlyCollection<GetUserDto>>(await _userRepository.GetItemsAsync(
            query.offset,
            query.limit,
            string.IsNullOrWhiteSpace(query.nameText)
                ? null
                : t => t.Login.Contains(query.nameText, StringComparison.InvariantCultureIgnoreCase),
            t => t.Id, null, cancellationToken));

    }
}

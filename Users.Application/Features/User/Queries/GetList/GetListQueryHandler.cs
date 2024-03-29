﻿using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Common.Domain.Users;

namespace Users.Application.Features.User.Queries.GetList;

public class GetListQueryHandler : IQueryHandler<GetListQuery, IReadOnlyCollection<GetUserDto>>
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;

    public GetListQueryHandler(
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
            query.Offset,
            query.Limit,
            string.IsNullOrWhiteSpace(query.Predicate)
                ? null
                : t => t.Login.Contains(query.Predicate),
            t => t.Id, null, cancellationToken));

    }
}

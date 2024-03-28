using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Dto;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using MediatR;
using Common.Domain.Users;

namespace Users.Application.Features.User.Queries.GetByIdQuery;

public class GetByIdQueryHandler : IQueryHandler<GetByIdQuery, GetUserDto>
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;

    public GetByIdQueryHandler(
        IRepository<ApplicationUser> userRepositiry,
        UsersMemoryCache cache,
        IMapper mapper) : base(cache.Cache, 3)
    {
        _userRepository = userRepositiry;
        _mapper = mapper;
    }

    public override async Task<GetUserDto> ExecQuery(GetByIdQuery query, CancellationToken cancellationToken)
    {
        return _mapper.Map<GetUserDto>(await _userRepository
            .SingleOrDefaultAsync(t => t.Id == query.Id, cancellationToken));
    }
}

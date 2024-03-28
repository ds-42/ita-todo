using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using Common.Domain.Users;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Application.Features.User.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GetUserDto>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(
        IRepository<ApplicationUser> users,
        ICurrentUserService currentUser,
        UsersMemoryCache cache,
        IMapper mapper)
    {
        _users = users;
        _currentUser = currentUser;
        _mapper = mapper;
        _cache = cache.Cache;
    }

    public async Task<GetUserDto> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        _currentUser.TestAccess(command.UserId);

        var item = await _users.SingleAsync(t => t.Id == command.UserId, cancellationToken);

        item.Login = command.User.Login;
        item.Password = PasswordHashUtils.Hash(command.User.Password);

        var result = await _users.UpdateAsync(item, cancellationToken);

        _cache.Clear();
        return _mapper.Map<GetUserDto>(result);
    }
}

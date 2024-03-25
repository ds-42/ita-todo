using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Users.Application.Features.User.Commands.UpdateUser;
using Users.Services.Dto;
using Users.Services.Utils;

namespace Users.Application.Features.User.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, GetUserDto>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;
    private readonly IMapper _mapper;

    public ChangePasswordCommandHandler(
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

    public async Task<GetUserDto> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        _currentUser.TestAccess(command.UserId);

        var item = await _users.SingleAsync(t => t.Id == command.UserId, cancellationToken);

        item.Password = PasswordHashUtils.Hash(command.Password);

        _cache.Clear();
        return _mapper.Map<GetUserDto>(await _users.UpdateAsync(item, cancellationToken));
    }
}

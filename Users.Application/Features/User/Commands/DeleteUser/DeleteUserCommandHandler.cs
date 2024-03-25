using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Users.Application.Features.User.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IRepository<ApplicationUser> _users;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;

    public DeleteUserCommandHandler(
        IRepository<ApplicationUser> users,
        ICurrentUserService currentUser,
        UsersMemoryCache cache)
    {
        _users = users;
        _currentUser = currentUser;
        _cache = cache.Cache;
    }

    public async Task<bool> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        _currentUser.TestAccess(command.UserId);

        var item = await _users.SingleAsync(t => t.Id == command.UserId, cancellationToken);


        bool result = await _users.DeleteAsync(item, cancellationToken);

        _cache.Clear();
        return result;
    }
}

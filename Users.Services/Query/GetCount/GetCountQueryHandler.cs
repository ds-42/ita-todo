using Common.Domain;
using Common.Repositories;
using Users.Services.Dto;

namespace Users.Services.Query.GetCount;

public class GetCountQueryHandler
{
    private readonly IRepository<ApplicationUser> _users;

    public GetCountQueryHandler(
        IRepository<ApplicationUser> userRepositiry)
    {
        _users = userRepositiry;
    }


    public async Task<int> RunAsync(BaseUsersFilter query, CancellationToken cancellationToken) 
    {
        if (string.IsNullOrWhiteSpace(query.nameText))
        {
            return await _users.CountAsync(cancellationToken: cancellationToken);
        }

        return await _users.CountAsync(t => t.Login.Contains(query.nameText), cancellationToken: cancellationToken);
    }
}

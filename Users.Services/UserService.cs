using Common.Domain;
using Common.Repositories;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepositiry;

    public UserService(IUserRepository userRepositiry) 
    {
        _userRepositiry = userRepositiry;
    }

    public IReadOnlyCollection<User> GetItems(int offset = 0, int limit = 10, string nameText = "") 
    {
        return _userRepositiry.GetItems(offset, limit, nameText);
    }

    public User? Get(int id)
    {
        return _userRepositiry.Find(id);
    }

    public User Create(User user) 
    {
        return _userRepositiry.Append(user);
    }

    public User? Update(User user)
    {
        return _userRepositiry.Update(user);
    }

    public User? Delete(int id)
    {
        return _userRepositiry.DeleteById(id);
    }
}

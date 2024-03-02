using Common.Domain;
using Common.Repositories;

namespace Users.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepositiry;

    public UserService(IRepository<User> userRepositiry) 
    {
        _userRepositiry = userRepositiry;
    }

    public IReadOnlyCollection<User> GetItems(int offset = 0, int limit = 10, string nameText = "") 
    {
        return _userRepositiry.GetItems(
            offset, 
            limit, 
            string.IsNullOrWhiteSpace(nameText)
                ? null 
                : t => t.Name.Contains(nameText, StringComparison.InvariantCultureIgnoreCase), 
            t => t.Id);
    }

    public User? GetById(int id)
    {
        return _userRepositiry.SingleOrDefault(t => t.Id == id);
    }

    public User Create(User user) 
    {
        user.Id = (_userRepositiry.GetItems().Max(t => t.Id as int?) ?? 0) + 1;
        return _userRepositiry.Add(user);
    }

    public User? Update(User user)
    {
        var item = GetById(user.Id);

        if (item == null) 
            return null;

        return _userRepositiry.Update(user);
    }

    public User? Delete(int id)
    {
        var item = GetById(id);

        if (item == null || _userRepositiry.Delete(item) == false)
            return null;

        return item;
    }
}

using Common.Domain;

namespace Users.Services;

public interface IUserService
{
    IReadOnlyCollection<User> GetItems(int offset = 0, int limit = 10, string nameText = "");
    User? GetById(int id);
    User Create(User user);
    User? Update(User user);
    User? Delete(int id);
}

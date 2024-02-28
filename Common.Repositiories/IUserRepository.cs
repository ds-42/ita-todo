using Common.Domain;

namespace Common.Repositories
{
    public interface IUserRepository
    {
        IReadOnlyCollection<User> GetItems(int? offset, int? limit, string? nameText);
        User Get(int id);
        User? Find(int id);
        User Append(User user);
        User? Update(User user);
        User? DeleteById(int id);
    }
}

using Common.Domain;
using Common.Domain.Exceptions;

namespace Common.Repositories;

public class UserRepository : IUserRepository
{
    protected static List<User> Items = new();

    public IReadOnlyCollection<User> GetItems(int? offset, int? limit, string? nameText)
    {
        IEnumerable<User> items = Items;

        if (!string.IsNullOrWhiteSpace(nameText))
        {
            items = items.Where(t => t.Name.Contains(nameText, StringComparison.InvariantCultureIgnoreCase));
        }

        items = items.OrderBy(t => t.Id);

        if (offset != null)
        {
            items = items.Skip(offset.Value);
        }

        limit ??= 10;

        items = items
            .Take(limit.Value)
            .ToArray();

        return (IReadOnlyCollection<User>)items;
    }

    public User Get(int id)
    {
        var item = Find(id);

        if (item == null)
            throw new InvalidUserException();

        return item;
    }

    public User? Find(int id)
    {
        return Items
            .SingleOrDefault(t => t.Id == id);
    }

    public User Append(User user)
    {
        var item = new User()
        {
            Id = (Items.Max(t => t.Id as int?) ?? 0) + 1,
            Name = user.Name,
        };

        Items.Add(item);

        return item;
    }

    public User? Update(User user)
    {
        var item = Find(user.Id);

        if (item != null)
        {
            item.Name = user.Name;
        }

        return item;
    }

    public User? DeleteById(int id)
    {
        var item = Find(id);

        if (item != null)
        {
            Items.Remove(item);
        }

        return item;
    }
}

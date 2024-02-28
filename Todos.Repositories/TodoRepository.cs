using Todos.Domain;

namespace Todos.Repositiories;

public class TodoRepository : ITodoRepository
{
    protected static List<Todo> Items = new();

    public IReadOnlyCollection<Todo> GetItems(int? offset, int? limit, string? labelText, int? ownerId)
    {
        IEnumerable<Todo> items = Items;

        if(ownerId != null && ownerId > 0)
            items = items.Where(t => t.OwnerId == ownerId);

        if (!string.IsNullOrWhiteSpace(labelText))
            items = items.Where(t => t.Label.Contains(labelText, StringComparison.InvariantCultureIgnoreCase));

        items = items.OrderBy(t => t.Id);

        if (offset != null)
        {
            items = items.Skip(offset.Value);
        }

        limit ??= 10;

        items = items
            .Take(limit.Value)
            .ToArray();

        return (IReadOnlyCollection<Todo>)items;
    }

    public Todo Get(int id)
    {
        var item = Find(id);

        if (item == null)
            throw new Exception("Invalid todo id");

        return item;
    }

    public Todo? Find(int id)
    {
        return Items
            .SingleOrDefault(t => t.Id == id);
    }

    public Todo Append(Todo todo)
    {
        var item = new Todo()
        {
            Id = Items.Max(t => t.Id) + 1,
            OwnerId = todo.OwnerId,
            IsDone = todo.IsDone,
            Label = todo.Label,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
        };

        Items.Add(item);

        return item;
    }

    public Todo? Update(Todo todo)
    {
        var item = Find(todo.Id);

        if (item != null)
        {
            item.OwnerId = todo.OwnerId;
            item.IsDone = todo.IsDone;
            item.Label = todo.Label;
            item.UpdateDate = DateTime.UtcNow;
        }

        return item;
    }

    public Todo? DeleteById(int id)
    {
        var item = Find(id); 

        if (item != null)
        {
            Items.Remove(item);
        }

        return item;
    }
}

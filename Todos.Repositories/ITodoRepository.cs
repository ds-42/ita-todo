using Todos.Domain;

namespace Todos.Repositiories;

public interface ITodoRepository
{
    IReadOnlyCollection<Todo> GetItems(int? offset, int? limit, string? labelText, int? ownerId);
    Todo Get(int id);
    Todo? Find(int id);
    Todo Append(Todo item);
    Todo? Update(Todo item);
    Todo? DeleteById(int id);
}

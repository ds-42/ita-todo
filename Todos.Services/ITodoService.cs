using Todos.Domain;

namespace Todos.Services;

public interface ITodoService
{
    IReadOnlyCollection<Todo> GetItems(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0);
    Todo? Get(int id);
    Todo Create(Todo todo);
    Todo? Update(Todo todo);
    Todo? Delete(int id);
    Todo? Done(int id);

}

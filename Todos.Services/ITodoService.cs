using Todos.Domain;
using Todos.Services.Dto;

namespace Todos.Services;

public interface ITodoService
{
    IReadOnlyCollection<Todo> GetItems(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0);
    Todo? Get(int id);
    Todo Create(CreateTodoDto todo);
    Todo? Update(UpdateTodoDto todo);
    Todo? Delete(int id);
    Todo? Done(int id);

}

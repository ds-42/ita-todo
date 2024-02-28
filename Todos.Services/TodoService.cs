using Common.Repositories;
using Todos.Domain;
using Todos.Repositiories;

namespace Todos.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepositiry;
    private readonly IUserRepository _userRepositiry;

    public TodoService(ITodoRepository todoRepository, IUserRepository userRepository) 
    {
        _todoRepositiry = todoRepository;
        _userRepositiry = userRepository;
    }

    public IReadOnlyCollection<Todo> GetItems(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0) 
    {
        return _todoRepositiry.GetItems(offset, limit, labelText, ownerId);
    }

    public Todo? Get(int id)
    {
        return _todoRepositiry.Find(id);
    }

    public Todo Create(Todo todo) 
    {
        _userRepositiry.Get(todo.OwnerId);

        return _todoRepositiry.Append(todo);
    }

    public Todo? Update(Todo todo)
    {
        _userRepositiry.Get(todo.OwnerId);

        return _todoRepositiry.Update(todo);
    }

    public Todo? Delete(int id)
    {
        return _todoRepositiry.DeleteById(id);
    }

    public Todo? Done(int id)
    {
        var item = Get(id);

        if (item != null)
        {
            item.IsDone = true;
            return Update(item);
        }

        return item;
    }

}

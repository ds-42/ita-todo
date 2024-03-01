using AutoMapper;
using Common.Domain.Exceptions;
using Common.Repositories;
using Todos.Domain;
using Todos.Repositiories;
using Todos.Services.Dto;

namespace Todos.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _todoRepositiry;
    private readonly IUserRepository _userRepositiry;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository todoRepository, IUserRepository userRepository, IMapper mapper) 
    {
        _todoRepositiry = todoRepository;
        _userRepositiry = userRepository;
        _mapper = mapper;
    }

    public IReadOnlyCollection<Todo> GetItems(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0) 
    {
        return _todoRepositiry.GetItems(offset, limit, labelText, ownerId);
    }

    public Todo? Get(int id)
    {
        return _todoRepositiry.Find(id);
    }

    public Todo Create(CreateTodoDto todo) 
    {
        var user = _userRepositiry.Find(todo.OwnerId);
        if (user == null)
            throw new InvalidUserException();

        var item = _mapper.Map<CreateTodoDto, Todo>(todo);

        return _todoRepositiry.Append(item);
    }

    public Todo? Update(UpdateTodoDto todo)
    {
        var user = _userRepositiry.Find(todo.OwnerId);
        if (user == null)
            throw new InvalidUserException();

        var item = _mapper.Map<UpdateTodoDto, Todo>(todo);

        return _todoRepositiry.Update(item);
    }

    public Todo? Delete(int id)
    {
        return _todoRepositiry.DeleteById(id);
    }

    public Todo? Done(int id)
    {
        var item = Get(id);

/**lai        if (item != null)
        {
            item.IsDone = true;
            return Update(item);
        }*/

        return item;
    }

}

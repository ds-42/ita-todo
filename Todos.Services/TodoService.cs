using AutoMapper;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Repositories;
using System.Linq.Expressions;
using System.Reflection;
using Todos.Domain;
using Todos.Services.Dto;

namespace Todos.Services;

public class TodoService : ITodoService
{
    private readonly IRepository<Todo> _todoRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public TodoService(
        IRepository<Todo> todoRepository, 
        IRepository<User> userRepository, 
        IMapper mapper) 
    {
        _todoRepository = todoRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public IReadOnlyCollection<Todo> GetItems(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0) 
    {
        var param = Expression.Parameter(typeof(Todo), nameof(Todo));
        Expression? body = null;

        if (ownerId > 0)
        { 
            var member = Expression.Property(param, nameof(Todo.OwnerId));
            var constant = Expression.Constant(ownerId);
            var expression = Expression.Equal(member, constant);
            body = body == null
                ? expression
                : Expression.AndAlso(body, expression);
        }

        if (!string.IsNullOrWhiteSpace(labelText))
        {
            var member = Expression.Property(param, nameof(Todo.Label));
            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string)});
            var expression = Expression.Constant(labelText, typeof(string));
            var containsMethodExpr = Expression.Call(member, method, expression);
            body = body == null
                ? expression
                : Expression.AndAlso(body, containsMethodExpr);
        }

        body ??= Expression.Constant(true);

        return _todoRepository.GetItems(
            offset, 
            limit, 
            Expression.Lambda<Func<Todo, bool>>(body, param), 
            t => t.Id);
    }

    public int Count(string labelText = "")
    {
        return string.IsNullOrWhiteSpace(labelText)
            ? _todoRepository.Count()
            : _todoRepository.Count(t => t.Label.Contains(labelText));
    }


    public Todo? GetById(int id)
    {
        return _todoRepository.SingleOrDefault(t => t.Id == id);
    }

    public Todo Create(CreateTodoDto todo) 
    {
        var user = _userRepository
            .SingleOrDefault(t => t.Id == todo.OwnerId);

        if (user == null)
            throw new InvalidUserException();

        var item = _mapper.Map<CreateTodoDto, Todo>(todo);

        item.Id = (_todoRepository.GetItems().Max(t => t.Id as int?) ?? 0) + 1;
        item.CreateDate = DateTime.UtcNow;
        item.UpdateDate = DateTime.UtcNow;

        return _todoRepository.Add(item);
    }

    public Todo? Update(UpdateTodoDto todo)
    {
        var user = _userRepository
            .SingleOrDefault(t => t.Id == todo.OwnerId);

        if (user == null)
            throw new InvalidUserException();

        var item = _mapper.Map<UpdateTodoDto, Todo>(todo);
        item.UpdateDate = DateTime.UtcNow;

        return _todoRepository.Update(item);
        // faq: Update не сработает так как у нас разные объекты
    }

    public Todo? Delete(int id)
    {
        var item = GetById(id);
        if (item == null || _todoRepository.Delete(item) == false)
            return null;

        return item;
    }

    public Todo? Done(int id)
    {
        var item = GetById(id);

        if (item != null)
        {
            item.IsDone = true;
            return _todoRepository.Update(item);
        }

        return item;
    }

}

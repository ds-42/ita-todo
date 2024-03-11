using AutoMapper;
using Common.Api.Exceptions;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Repositories;
using FluentValidation;
using Serilog;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Todos.Domain;
using Todos.Services.Dto;
using Todos.Services.Validators;

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
            MethodInfo method = typeof(string).GetMethod("Equals", new[] { typeof(string)});
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


    public async Task<Todo> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _todoRepository
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (item == null)
            throw new InvalidTodoException(id);

        return item;
    }

    public Todo Create(CreateTodoDto todo) 
    {
        var user = _userRepository
            .SingleOrDefault(t => t.Id == todo.OwnerId);
        
        if (user == null)
            throw new InvalidUserException(todo.OwnerId);

        var item = _mapper.Map<CreateTodoDto, Todo>(todo);

        item.CreateDate = DateTime.UtcNow;
        item.UpdateDate = DateTime.UtcNow;

        item = _todoRepository.Add(item);

        Log.Information($"Добавлена новая запись: {JsonSerializer.Serialize(item)}");

        return item;
    }

    public Todo Update(UpdateTodoDto todo)
    {
        var user = _userRepository
            .SingleOrDefault(t => t.Id == todo.OwnerId);

        if (user == null)
            throw new InvalidUserException(todo.OwnerId);

        var item = GetByIdAsync(todo.Id).Result;
        _mapper.Map<UpdateTodoDto, Todo>(todo, item);
        item.UpdateDate = DateTime.UtcNow;

        item = _todoRepository.Update(item);

        Log.Information($"Запись изменена: {JsonSerializer.Serialize(item)}");

        return item;
    }

    public Todo Delete(int id)
    {
        var item = GetByIdAsync(id).Result;
        _todoRepository.Delete(item);

        Log.Information($"Запись удалена: {JsonSerializer.Serialize(item)}");
        return item;
    }

    public Todo Done(int id)
    {
        var item = GetByIdAsync(id).Result;

        item.IsDone = true;
        _todoRepository.Update(item);

        Log.Information($"Признак выполнения изменен: {JsonSerializer.Serialize(item)}");

        return item;
    }

}

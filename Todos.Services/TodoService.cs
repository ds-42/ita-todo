using AutoMapper;
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
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly IMapper _mapper;

    public TodoService(
        IRepository<Todo> todoRepository, 
        IRepository<ApplicationUser> userRepository,
        IMapper mapper) 
    {
        _todoRepository = todoRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<Todo>> GetItemsAsync(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0, CancellationToken cancellationToken = default) 
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

        return await _todoRepository.GetItemsAsync(
            offset, 
            limit, 
            Expression.Lambda<Func<Todo, bool>>(body, param), 
            t => t.Id, false, cancellationToken);
    }

    public async Task<int> CountAsync(string labelText = "", CancellationToken cancellationToken = default)
    {
        return string.IsNullOrWhiteSpace(labelText)
            ? await _todoRepository.CountAsync(null, cancellationToken)
            : await _todoRepository.CountAsync(t => t.Label.Contains(labelText), cancellationToken);
    }


    public async Task<Todo> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _todoRepository
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (item == null)
            throw new InvalidTodoException(id);

        return item;
    }

    public async Task<Todo> CreateAsync(CreateTodoDto todo, CancellationToken cancellationToken = default) 
    {
        var user = await _userRepository
            .SingleOrDefaultAsync(t => t.Id == todo.OwnerId, cancellationToken);
        
        if (user == null)
            throw new InvalidUserException(todo.OwnerId);

        var item = _mapper.Map<CreateTodoDto, Todo>(todo);

        item.CreateDate = DateTime.UtcNow;
        item.UpdateDate = DateTime.UtcNow;

        item = await _todoRepository.AddAsync(item, cancellationToken);

        Log.Information($"Добавлена новая запись: {JsonSerializer.Serialize(item)}");

        return item;
    }

    public async Task<Todo> UpdateAsync(UpdateTodoDto todo, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository
            .SingleOrDefaultAsync(t => t.Id == todo.OwnerId, cancellationToken);

        if (user == null)
            throw new InvalidUserException(todo.OwnerId);

        var item = await GetByIdAsync(todo.Id, cancellationToken);
        _mapper.Map<UpdateTodoDto, Todo>(todo, item);
        item.UpdateDate = DateTime.UtcNow;

        item = await _todoRepository.UpdateAsync(item, cancellationToken);

        Log.Information($"Запись изменена: {JsonSerializer.Serialize(item)}");

        return item;
    }

    public async Task<Todo> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await GetByIdAsync(id, cancellationToken);
        await _todoRepository.DeleteAsync(item, cancellationToken);

        Log.Information($"Запись удалена: {JsonSerializer.Serialize(item)}");
        return item;
    }

    public async Task<Todo> DoneAsync(int id, CancellationToken cancellationToken = default)    
    {
        var item = await GetByIdAsync(id, cancellationToken);

        item.IsDone = true;
        await _todoRepository.UpdateAsync(item, cancellationToken);

        Log.Information($"Признак выполнения изменен: {JsonSerializer.Serialize(item)}");

        return item;
    }

}

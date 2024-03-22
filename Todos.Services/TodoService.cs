using AutoMapper;
using Common.Api.Services;
using Common.Api.Extensions;
using Common.Domain;
using Common.Domain.Exceptions;
using Common.Repositories;
using FluentValidation;
using Serilog;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using Todos.Domain;
using Todos.Services.Dto;
using Todos.Services.Validators;
using Common.BL.Extensions;

namespace Todos.Services;

public class TodoService : ITodoService
{
    private readonly IRepository<Todo> _todoRepository;
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public TodoService(
        IRepository<Todo> todoRepository, 
        IRepository<ApplicationUser> userRepository,
        ICurrentUserService currentUser,
        IMapper mapper) 
    {
        _todoRepository = todoRepository;
        _userRepository = userRepository;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<GetTodoDto>> GetItemsAsync(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0, CancellationToken cancellationToken = default) 
    {
        var param = Expression.Parameter(typeof(Todo), nameof(Todo));
        Expression? body = null;

        if (!_currentUser.IsAdmin)
            ownerId = _currentUser.UserId;

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

        return _mapper.Map<IReadOnlyCollection<GetTodoDto>>(await _todoRepository.GetItemsAsync(
            offset, 
            limit, 
            Expression.Lambda<Func<Todo, bool>>(body, param), 
            t => t.Id, false, cancellationToken));
    }

    public async Task<int> CountAsync(string labelText = "", CancellationToken cancellationToken = default)
    {
        return string.IsNullOrWhiteSpace(labelText)
            ? await _todoRepository.CountAsync(null, cancellationToken)
            : await _todoRepository.CountAsync(t => t.Label.Contains(labelText), cancellationToken);
    }

    protected async Task<Todo> GetRecordAsync(int id, CancellationToken cancellationToken = default) 
    {
        var item = await _todoRepository
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (item == null)
            throw new InvalidTodoException(id);

        return item;
    }

    public async Task<GetTodoDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await GetRecordAsync(id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        return _mapper.Map<GetTodoDto>(item);
    }

    protected async Task<ApplicationUser> TestOwnerByIdAsync(int userId, CancellationToken cancellationToken = default) 
    {
        var user = await _userRepository
            .SingleOrDefaultAsync(t => t.Id == userId, cancellationToken);

        if (user == null)
            throw new InvalidUserException(userId);

        return user;
    }

    public async Task<GetTodoDto> CreateAsync(CreateTodoDto todo, CancellationToken cancellationToken = default) 
    {
        if (!_currentUser.IsAdmin)
            todo.OwnerId = _currentUser.UserId;

        await TestOwnerByIdAsync(todo.OwnerId, cancellationToken);

        var item = _mapper.Map<CreateTodoDto, Todo>(todo);

        item.CreateDate = DateTime.UtcNow;
        item.UpdateDate = DateTime.UtcNow;

        item = await _todoRepository.AddAsync(item, cancellationToken);

        Log.Information($"Добавлена новая запись: {item.JsonSerialize()}");

        return _mapper.Map<GetTodoDto>(item);
    }

    public async Task<GetTodoDto> UpdateAsync(UpdateTodoDto todo, CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAdmin)
            todo.OwnerId = _currentUser.UserId;

        await TestOwnerByIdAsync(todo.OwnerId, cancellationToken);

        var item = await GetRecordAsync(todo.Id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        _mapper.Map(todo, item);
        item.UpdateDate = DateTime.UtcNow;

        item = await _todoRepository.UpdateAsync(item, cancellationToken);

//        Log.Information($"Запись изменена: {JsonSerializer.JsonSerialize(item)}");

        return _mapper.Map<GetTodoDto>(item);
    }

    public async Task<GetTodoDto> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await GetRecordAsync(id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        await _todoRepository.DeleteAsync(item, cancellationToken);

//        Log.Information($"Запись удалена: {JsonSerializer.JsonSerialize(item)}");

        return _mapper.Map<GetTodoDto>(item);
    }

    public async Task<GetTodoDto> DoneAsync(int id, CancellationToken cancellationToken = default)    
    {
        var item = await GetRecordAsync(id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        item.IsDone = true;
        await _todoRepository.UpdateAsync(item, cancellationToken);

//        Log.Information($"Признак выполнения изменен: {JsonSerializer.JsonSerialize(item)}");

        return _mapper.Map<GetTodoDto>(item);
    }

}

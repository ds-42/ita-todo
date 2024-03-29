using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using Common.Domain.Users;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Todos.Application.Dto;
using Todos.Application.Features.Todo.Commands.UpdateTodo;
using TodoModel = Common.Domain.Todo;

namespace Todos.Application.Features.Todo.Commands.CreateTodo;

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, GetTodoDto>
{
    private readonly IRepository<TodoModel> _todos;
    private readonly IRepository<ApplicationUser> _users;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;
    private readonly IMapper _mapper;

    public UpdateTodoCommandHandler(
        IRepository<TodoModel> todos,
        IRepository<ApplicationUser> users,
        ICurrentUserService currentUser,
        TodosMemoryCache cache,
        IMapper mapper)
    {
        _todos = todos;
        _users = users;
        _currentUser = currentUser;
        _mapper = mapper;
        _cache = cache.Cache;
    }

    public async Task<GetTodoDto> Handle(UpdateTodoCommand command, CancellationToken cancellationToken)
    {
        var ownerId = command.OwnerId;
        if (!_currentUser.IsAdmin)
            ownerId = _currentUser.UserId;

        var user = await _users.SingleAsync(t => t.Id == ownerId, cancellationToken);

        var item = await _todos.SingleAsync(t => t.Id == command.Id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        _mapper.Map(command, item);
        item.UpdateDate = DateTime.UtcNow;

        item = await _todos.UpdateAsync(item, cancellationToken);

         Log.Information($"Запись изменена: {item.JsonSerialize()}");

        _cache.Clear();
        return _mapper.Map<GetTodoDto>(item);
    }
}

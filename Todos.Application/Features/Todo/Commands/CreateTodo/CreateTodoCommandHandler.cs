using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Exceptions;
using Common.Domain;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Todos.Application.Dto;
using Users.Services.Utils;

namespace Todos.Application.Features.Todo.Commands.CreateTodo;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, GetTodoDto>
{
    private readonly IRepository<Common.Domain.Todo> _todos;
    private readonly IRepository<ApplicationUser> _users;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;
    private readonly IMapper _mapper;

    public CreateTodoCommandHandler(
        IRepository<Common.Domain.Todo> todos,
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

    public async Task<GetTodoDto> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var ownerId = command.OwnerId;
        if (!_currentUser.IsAdmin)
            ownerId = _currentUser.UserId;

        var user = await _users.SingleAsync(t => t.Id == ownerId, cancellationToken);

        var result = _mapper.Map<CreateTodoCommand, Common.Domain.Todo>(command);

        result.CreateDate = DateTime.UtcNow;
        result.UpdateDate = DateTime.UtcNow;

        result = await _todos.AddAsync(result, cancellationToken);

//        Log.Information($"Добавлена новая запись: {item.JsonSerialize()}");

        _cache.Clear();
        return _mapper.Map<GetTodoDto>(result);
    }
}

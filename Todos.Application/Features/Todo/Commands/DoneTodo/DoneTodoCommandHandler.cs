using AutoMapper;
using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Todos.Application.Dto;

namespace Todos.Application.Features.Todo.Commands.DoneTodo;

public class DoneTodoCommandHandler : IRequestHandler<DoneTodoCommand, GetTodoDto>
{
    private readonly IRepository<Common.Domain.Todo> _todos;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;
    private readonly IMapper _mapper;

    public DoneTodoCommandHandler(
        IRepository<Common.Domain.Todo> todos,
        ICurrentUserService currentUser,
        TodosMemoryCache cache,
        IMapper mapper)
    {
        _todos = todos;
        _currentUser = currentUser;
        _mapper = mapper;
        _cache = cache.Cache;
    }

    public async Task<GetTodoDto> Handle(DoneTodoCommand command, CancellationToken cancellationToken)
    {
        var item = await _todos.SingleAsync(t => t.Id == command.Id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        item.IsDone = true;
        await _todos.UpdateAsync(item, cancellationToken);

//        Log.Information($"Признак выполнения изменен: {JsonSerializer.JsonSerialize(item)}");

        _cache.Clear();
        return _mapper.Map<GetTodoDto>(item);
    }
}

using Common.Application.Abstractions;
using Common.Application.Abstractions.Persistence;
using Common.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using TodoModel = Common.Domain.Todo;

namespace Todos.Application.Features.Todo.Commands.DeleteTodo;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
{
    private readonly IRepository<TodoModel> _todos;
    private readonly ICurrentUserService _currentUser;
    private readonly MemoryCache _cache;

    public DeleteTodoCommandHandler(
        IRepository<TodoModel> todos,
        ICurrentUserService currentUser,
        TodosMemoryCache cache)
    {
        _todos = todos;
        _currentUser = currentUser;
        _cache = cache.Cache;
    }

    public async Task<bool> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        var item = await _todos.SingleAsync(t => t.Id == command.Id, cancellationToken);

        _currentUser.TestAccess(item.OwnerId);

        bool result = await _todos.DeleteAsync(item, cancellationToken);

        Log.Information($"Запись удалена: {item.JsonSerialize()}");

        _cache.Clear();
        return result;
    }
}

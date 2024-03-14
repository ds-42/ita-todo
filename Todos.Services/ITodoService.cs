using Todos.Domain;
using Todos.Services.Dto;

namespace Todos.Services;

public interface ITodoService
{
    Task<IReadOnlyCollection<Todo>> GetItemsAsync(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string labelText = "", CancellationToken cancellationToken = default);
    Task<Todo> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Todo> CreateAsync(CreateTodoDto todo, CancellationToken cancellationToken = default);
    Task<Todo> UpdateAsync(UpdateTodoDto todo, CancellationToken cancellationToken = default);
    Task<Todo> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<Todo> DoneAsync(int id, CancellationToken cancellationToken = default);

}

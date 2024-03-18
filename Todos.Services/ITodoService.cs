using Todos.Domain;
using Todos.Services.Dto;

namespace Todos.Services;

public interface ITodoService
{
    Task<IReadOnlyCollection<GetTodoDto>> GetItemsAsync(int offset = 0, int limit = 10, string labelText = "", int ownerId = 0, CancellationToken cancellationToken = default);
    Task<int> CountAsync(string labelText = "", CancellationToken cancellationToken = default);
    Task<GetTodoDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GetTodoDto> CreateAsync(CreateTodoDto todo, CancellationToken cancellationToken = default);
    Task<GetTodoDto> UpdateAsync(UpdateTodoDto todo, CancellationToken cancellationToken = default);
    Task<GetTodoDto> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<GetTodoDto> DoneAsync(int id, CancellationToken cancellationToken = default);

}

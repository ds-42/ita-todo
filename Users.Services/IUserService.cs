using Common.Domain;
using Users.Services.Dto;

namespace Users.Services;

public interface IUserService
{
    Task<IReadOnlyCollection<GetUserDto>> GetItemsAsync(int offset = 0, int limit = 10, string nameText = "", CancellationToken cancellationToken = default);
    Task<GetUserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<GetUserDto> CreateAsync(CreateUserDto user, CancellationToken cancellationToken = default);
    Task<GetUserDto?> UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

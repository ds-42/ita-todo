using Users.Services.Dto;

namespace Users.Services;

public interface IUserService
{
    Task<GetUserDto?> GetByIdOrDefaultAsync(int id, CancellationToken cancellationToken = default);
    Task<GetUserDto> UpdateAsync(int id, UpdateUserDto user, CancellationToken cancellationToken = default);
    Task<GetUserDto> ChangePasswordAsync(int id, string password, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}

using Users.Services.Dto;

namespace Users.Services;

public interface IAuthService
{
    public Task<string> GetJwtToken(AuthDto userDto, CancellationToken cancellationToken);
}

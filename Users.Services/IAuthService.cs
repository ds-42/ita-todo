using Users.Services.Dto;

namespace Users.Services;

public interface IAuthService
{
    public Task<JwtTokenDto> GetJwtTokenAsync(AuthDto userDto, CancellationToken cancellationToken);
    public Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}

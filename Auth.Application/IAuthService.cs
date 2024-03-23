using Auth.Application.Dto;

namespace Auth.Application;

public interface IAuthService
{
    public Task<JwtTokenDto> GetJwtTokenAsync(AuthDto userDto, CancellationToken cancellationToken);
    public Task<JwtTokenDto> GetJwtTokenByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
}

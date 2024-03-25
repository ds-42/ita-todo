using Auth.Application.Dto;
using MediatR;

namespace Auth.Application.Features.Auth.Commands.GetJwtTokenByRefreshToken;

public class GetJwtTokenByRefreshTokenCommand : IRequest<JwtTokenDto>
{
    public string RefreshToken { get; set; } = default!;
}



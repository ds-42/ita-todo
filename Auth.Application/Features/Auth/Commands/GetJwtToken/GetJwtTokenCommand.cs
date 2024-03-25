using Auth.Application.Dto;
using MediatR;

namespace Auth.Application.Features.Auth.Commands.GetJwtToken;

public class GetJwtTokenCommand : IRequest<JwtTokenDto>
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
}



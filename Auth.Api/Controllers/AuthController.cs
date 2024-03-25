using Auth.Application;
using Auth.Application.Dto;
using Auth.Application.Features.Auth.Commands.GetJwtToken;
using Auth.Application.Features.Auth.Commands.GetJwtTokenByRefreshToken;
using Common.Application.Controllers;
using Common.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Features.User.Commands.CreateUser;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Auth.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : BaseController
{
    public AuthController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost("CreateJwtToken")]
    public async Task<IActionResult> CreateJwtToken(GetJwtTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await ExecCommandAsync(command, cancellationToken);

        return Ok(token);
    }

    [HttpPost("CreateJwtTokenByRefreshToken")]
    public async Task<IActionResult> CreateJwtTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var command = new GetJwtTokenByRefreshTokenCommand() { RefreshToken = refreshToken };

        var token = await ExecCommandAsync(command, cancellationToken);

        return Ok(token);
    }

}

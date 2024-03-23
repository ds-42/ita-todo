using Auth.Application;
using Auth.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    protected readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost("CreateJwtToken")]
    public async Task<IActionResult> CreateJwtToken(AuthDto authDto, CancellationToken cancellationToken)
    {
        var token = await _authService.GetJwtTokenAsync(authDto, cancellationToken);

        return Ok(token);
    }

    [HttpPost("CreateJwtTokenByRefreshToken")]
    public async Task<IActionResult> CreateJwtTokenByRefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await _authService.GetJwtTokenByRefreshTokenAsync(refreshToken, cancellationToken);

        return Ok(token);
    }

}

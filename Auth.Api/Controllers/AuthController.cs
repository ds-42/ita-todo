using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Services;
using Users.Services.Dto;

namespace Auth.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    protected readonly IAuthService _authService;
    protected readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
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

    [Authorize]
    [HttpGet("GetMyInfo")]
    public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
    {
        var curentUserId = User.FindFirst(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdOrDefaultAsync(int.Parse(curentUserId.Value), cancellationToken);
        return Ok(user);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("IAmAdmin")]
    public async Task<IActionResult> IAmAdmin(CancellationToken cancellationToken)
    {
        var curentUserId = User.FindFirst(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdOrDefaultAsync(int.Parse(curentUserId.Value), cancellationToken);
        return Ok(user);
    }

}

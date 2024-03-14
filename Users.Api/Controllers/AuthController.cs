using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Users.Services;
using Users.Services.Dto;

namespace Users.Api.Controllers;

[ApiController]
[Route("users")]
public class AuthController : ControllerBase
{
    protected readonly IAuthService _authService;
    protected readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }


    [HttpPost("GetJwtToken")]
    public async Task<IActionResult> GetJwtToken(AuthDto authDto, CancellationToken cancellationToken)
    {
        var token = await _authService.GetJwtToken(authDto, cancellationToken);

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

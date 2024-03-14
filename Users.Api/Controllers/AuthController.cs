using Microsoft.AspNetCore.Mvc;
using Users.Services;
using Users.Services.Dto;

namespace Users.Api.Controllers;

[ApiController]
[Route("users")]
public class AuthController : ControllerBase
{
    protected readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost]
    public async Task<IActionResult> GetJwtToken(AuthDto authDto, CancellationToken cancellationToken)
    {
        var token = await _authService.GetJwtToken(authDto, cancellationToken);

        return Ok(token);
    }

}

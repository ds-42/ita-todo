using Microsoft.AspNetCore.Mvc;
using Users.Services;
using Users.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using Users.Services.Command.CreateUser;
using Users.Services.Query.GetList;
using Users.Services.Query.GetCount;

namespace Users.Api.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    protected readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetListQuery getListQuery,
        [FromServices] GetListQueryHandler getListQueryHandler,
        [FromServices] GetCountQueryHandler getCountQueryHandler,
        CancellationToken cancellationToken = default)
    {
        var items = await getListQueryHandler.RunAsync(getListQuery, cancellationToken);
        var count = await getCountQueryHandler.RunAsync(getListQuery, cancellationToken);

        HttpContext.Response.Headers.Append("X-Total-Count", count.ToString());
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken = default)
    {
        var item = await _userService.GetByIdOrDefaultAsync(id, cancellationToken);

        if (item == null)
        {
            return NotFound($"users/{id}");
        }

        return Ok(item);
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        CreateUserCommand createUserRequest, 
        [FromServices] CreateUserCommandHandler createUserCommandHandler, 
        CancellationToken cancellationToken = default)
    {
        var item = await createUserCommandHandler.RunAsync(createUserRequest, cancellationToken);

        return Created($"users/{item.Id}", item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto user, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.UpdateAsync(id, user, cancellationToken));
    }

    [HttpPatch("{id}/password")]
    public async Task<IActionResult> ChangePassword(int id, string password, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.ChangePasswordAsync(id, password, cancellationToken));
    }
    /*

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] int id, CancellationToken cancellationToken = default)
        {
            if(await _authService.DeleteAsync(id, cancellationToken) == false)
                return NotFound($"{id}");

            return Ok();
        }*/
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Users.Application.Query.GetList;
using Users.Application.Query.GetCount;
using Users.Application.Command.CreateUser;
using MediatR;

namespace Users.Api.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetListQuery getListQuery,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var items = await mediator.Send(getListQuery, cancellationToken);
        var count = await mediator.Send(getListQuery, cancellationToken);

        HttpContext.Response.Headers.Append("X-Total-Count", count.ToString());
        return Ok(items);
    }

/*    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken = default)
    {
        var item = await _userService.GetByIdOrDefaultAsync(id, cancellationToken);

        if (item == null)
        {
            return NotFound($"users/{id}");
        }

        return Ok(item);
    }*/


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        CreateUserCommand createUserRequest, 
        IMediator mediator, 
        CancellationToken cancellationToken = default)
    {
        var item = await mediator.Send(createUserRequest, cancellationToken);

        return Created($"users/{item.Id}", item);
    }

/*    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserDto user, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.UpdateAsync(id, user, cancellationToken));
    }

    [HttpPatch("{id}/password")]
    public async Task<IActionResult> ChangePassword(int id, string password, CancellationToken cancellationToken = default)
    {
        return Ok(await _userService.ChangePasswordAsync(id, password, cancellationToken));
    }
    

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] int id, CancellationToken cancellationToken = default)
    {
        if(await _authService.DeleteAsync(id, cancellationToken) == false)
            return NotFound($"{id}");

        return Ok();
    }*/
}

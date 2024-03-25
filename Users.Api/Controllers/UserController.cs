using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Common.Application.Controllers;
using Users.Application.Features.User.Queries.GetList;
using Users.Application.Features.User.Commands.CreateUser;
using Users.Application.Features.User.Queries.GetCount;
using Users.Application.Features.User.Queries.GetByIdQuery;
using Users.Application.Dto;
using Users.Application.Features.User.Commands.UpdateUser;
using Users.Application.Features.User.Commands.DeleteUser;

namespace Users.Api.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UserController : BaseController
{

    public UserController(IMediator mediator) : base(mediator) 
    {
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetListQuery getListQuery,
        CancellationToken cancellationToken)
    {
        var getCountQuery = new GetCountQuery() { Predicate = getListQuery.Predicate };

        var items = await ExecQueryAsync(getListQuery, cancellationToken);
        var count = await ExecQueryAsync(getCountQuery, cancellationToken);

        HttpContext.Response.Headers.Append("X-Total-Count", count.ToString());
        return Ok(items);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken = default)
    {
        var item = await ExecQueryAsync(new GetByIdQuery(id), cancellationToken);

        return Ok(item);
    }


    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        CreateUserCommand createUserCommand, 
        CancellationToken cancellationToken = default)
    {
        var item = await ExecCommandAsync(createUserCommand, cancellationToken);

        return Created($"users/{item.Id}", item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(
        int id,
        SetUserDto user,
        CancellationToken cancellationToken = default)
    {
        var updateUserCommand = new UpdateUserCommand(){ UserId=id, User = user };

        var item = await ExecCommandAsync(updateUserCommand, cancellationToken);

        return Ok(item);
    }

    [HttpPatch("{id}/password")]
    public async Task<IActionResult> ChangePassword(int id, string password, CancellationToken cancellationToken = default)
    {
        var changePasswordCommand = new ChangePasswordCommand() { UserId = id, Password = password };
        var item = await ExecCommandAsync(changePasswordCommand, cancellationToken);
        return Ok(item);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromBody] int id, CancellationToken cancellationToken = default)
    {
        var deleteUserCommand = new DeleteUserCommand() { UserId = id };
        var result = await ExecCommandAsync(deleteUserCommand, cancellationToken);
        return Ok(result);
    }
}


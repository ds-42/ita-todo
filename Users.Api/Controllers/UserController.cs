using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Common.Application.Controllers;
using Users.Application.Features.User.Queries.GetList;
using Users.Application.Features.User.Commands.CreateUser;
using Users.Application.Features.User.Queries.GetCount;
using AutoMapper;
using Common.Application.Abstractions.Persistence;
using Common.Application.Abstractions;
using Common.Application.Exceptions;
using Common.Domain.Exceptions;
using Common.Domain;
using Users.Services.Dto;
using Users.Services.Utils;
using Users.Application.Features.User.Queries.GetByIdQuery;

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
        var getCountQuery = new GetCountQuery() { nameText = getListQuery.nameText };

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
        CreateUserCommand createUserRequest, 
        CancellationToken cancellationToken = default)
    {
        var item = await ExecCommandAsync(createUserRequest, cancellationToken);

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


/*public class UserService : IUserService
{
    private readonly IRepository<ApplicationUser> _userRepository;
    private readonly ICurrentUserService _currentUser;
    private readonly IMapper _mapper;

    public UserService(
        IRepository<ApplicationUser> userRepositiry,
        ICurrentUserService currentUser,
        IMapper mapper)
    {
        _userRepository = userRepositiry;
        _currentUser = currentUser;
        _mapper = mapper;
    }



    public async Task<GetUserDto> UpdateAsync(int id, UpdateUserDto user, CancellationToken cancellationToken = default)
    {
        _currentUser.TestAccess(id);

        var item = await GetUserAsync(id, cancellationToken);

        item.Login = user.Login;
        item.Password = PasswordHashUtils.Hash(user.Password);

        return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(item, cancellationToken));
    }

    public async Task<GetUserDto> ChangePasswordAsync(int id, string password, CancellationToken cancellationToken = default)
    {
        _currentUser.TestAccess(id);

        var item = await GetUserAsync(id, cancellationToken);

        item.Password = PasswordHashUtils.Hash(password);

        return _mapper.Map<GetUserDto>(await _userRepository.UpdateAsync(item, cancellationToken));
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = await _userRepository.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (item == null)
            throw NotFoundException.Create(new { Id = id });

        return await _userRepository.DeleteAsync(item, cancellationToken);
    }
}
*/
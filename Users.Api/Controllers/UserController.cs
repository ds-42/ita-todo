using Microsoft.AspNetCore.Mvc;
using Common.Domain;
using Users.Services;
using Users.Services.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto user, CancellationToken cancellationToken = default)
    {
        var item = await _userService.CreateAsync(user, cancellationToken);

        return Created($"users/{item.Id}", item);
    }

    [HttpGet]
    public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken) 
    {
        var curentUserId = User.FindFirst(ClaimTypes.NameIdentifier);
        var user = await _userService.GetByIdOrDefaultAsync(int.Parse(curentUserId.Value), cancellationToken);
        return Ok(user);
    }


    /*
        [HttpGet]
        public async Task<IActionResult> Get(int offset = 0, int limit = 10, string nameText = "", CancellationToken cancellationToken = default)
        {
            var items = await _userService.GetItemsAsync(offset, limit, nameText, cancellationToken);
            return Ok(items);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken = default)
        {
            var item = await _userService.GetByIdOrDefaultAsync(id, cancellationToken);

            if (item == null)
            {
                return NotFound($"/{id}");
            }

            return Ok(item);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user, CancellationToken cancellationToken = default)
        {
            user.Id = id;
            var item = await _authService.UpdateAsync(user, cancellationToken);

            if (item == null)
                return NotFound($"{id}");

            return Ok(item);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] int id, CancellationToken cancellationToken = default)
        {
            if(await _authService.DeleteAsync(id, cancellationToken) == false)
                return NotFound($"{id}");

            return Ok();
        }*/
}

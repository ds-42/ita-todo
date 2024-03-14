using Microsoft.AspNetCore.Mvc;
using Common.Domain;
using Users.Services;

namespace Users.Api.Controllers;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    protected readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public IActionResult Get(int offset = 0, int limit = 10, string nameText = "")
    {
        var items = _userService.GetItems(offset, limit, nameText);
        return Ok(items);
    }


    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var item = _userService.GetByIdAsync(id);

        if (item == null)
        {
            return NotFound($"/{id}");
        }

        return Ok(item);
    }



    [HttpPost]
    public IActionResult AddUser(User user)
    {
        var item = _userService.Create(user);

        return Created($"/{item.Id}", item);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, User user)
    {
        user.Id = id;
        var item = _userService.UpdateAsync(user);

        if (item == null)
            return NotFound($"{id}");

        return Ok(item);
    }


    [HttpDelete]
    public IActionResult DeleteUser([FromBody] int id)
    {
        var item = _userService.DeleteAsync(id);

        if (item == null)
            return NotFound($"{id}");

        return Ok();
    }
}

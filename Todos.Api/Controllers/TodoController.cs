using Microsoft.AspNetCore.Mvc;
using Todos.Domain;
using Todos.Services;
using Todos.Services.Dto;

namespace Todos.Api.Controllers;

[ApiController]
[Route("todos")]
public class TodoController : ControllerBase
{
    protected readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }


    [HttpGet]
    public IActionResult Get(int offset = 0, int limit = 10, string label_text = "", int owner_id = 0)
    {
        var items = _todoService.GetItems(offset, limit, label_text, owner_id);
        return Ok(items);
    }


    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var item = _todoService.Get(id);

        if (item == null)
        {
            return NotFound($"/{id}");
        }

        return Ok(item);
    }

    protected IActionResult GetIsDoneState(Todo item) 
    {
        return Ok(new
        {
            item.Id,
            item.IsDone,
        });
    }


    [HttpGet("{id}/IsDone")]
    public IActionResult GetIsDone(int id)
    {
        var item = _todoService.Get(id);

        if (item == null)
            return NotFound();

        return GetIsDoneState(item);
    }


    [HttpPost]
    public IActionResult AddItem(CreateTodoDto item)
    {
        var rec = _todoService.Create(item);

        return Created($"todos/{rec.Id}", rec);
    }


    [HttpPut("{id}")]
    public IActionResult UpdateItem(int id, UpdateTodoDto item)
    {
        item.Id = id;
        var rec = _todoService.Update(item);

        if (rec == null)
            return NotFound($"{id}");

        return Ok(rec);
    }


    [HttpPatch("{id}/IsDone")]
    public IActionResult DoneItem(int id)
    {
        var item = _todoService.Done(id);

        if (item == null)
            return NotFound();

        return GetIsDoneState(item);
    }


    [HttpDelete]
    public IActionResult DeleteItem([FromBody] int id)
    {
        var item = _todoService.Delete(id);

        if (item == null)
            return NotFound($"{id}");

        return Ok();
    }
}

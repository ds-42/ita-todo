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
        int count = _todoService.Count(label_text);
        HttpContext.Response.Headers
            .Append("X-Total-Count", count.ToString());

        return Ok(items);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var item = await _todoService.GetByIdAsync(id, cancellationToken);

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
    public async Task<IActionResult> GetIsDone(int id)
    {
        var item = await _todoService.GetByIdAsync(id);

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

        return Ok(rec);
    }


    [HttpPatch("{id}/IsDone")]
    public IActionResult DoneItem(int id)
    {
        var item = _todoService.Done(id);

        return GetIsDoneState(item);
    }


    [HttpDelete]
    public IActionResult DeleteItem([FromBody] int id)
    {
        _todoService.Delete(id);

        return Ok();
    }
}

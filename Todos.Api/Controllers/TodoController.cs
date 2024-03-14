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
    public async Task<IActionResult> Get(int offset = 0, int limit = 10, string label_text = "", int owner_id = 0, CancellationToken cancellationToken = default)
    {
        var items = await _todoService.GetItemsAsync(offset, limit, label_text, owner_id, cancellationToken);
        int count = await _todoService.CountAsync(label_text, cancellationToken);
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
    public async Task<IActionResult> GetIsDone(int id, CancellationToken cancellationToken)
    {
        var item = await _todoService.GetByIdAsync(id, cancellationToken);

        return GetIsDoneState(item);
    }


    [HttpPost]
    public async Task<IActionResult> AddItem(CreateTodoDto item, CancellationToken cancellationToken)
    {
        var rec = await _todoService.CreateAsync(item, cancellationToken);

        return Created($"todos/{rec.Id}", rec);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(int id, UpdateTodoDto item, CancellationToken cancellationToken)
    {
        item.Id = id;
        var rec = await _todoService.UpdateAsync(item,cancellationToken);

        return Ok(rec);
    }


    [HttpPatch("{id}/IsDone")]
    public async Task<IActionResult> DoneItem(int id, CancellationToken cancellationToken)
    {
        var item = await _todoService.DoneAsync(id, cancellationToken);

        return GetIsDoneState(item);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteItem([FromBody] int id, CancellationToken cancellationToken)
    {
        await _todoService.DeleteAsync(id, cancellationToken);

        return Ok();
    }
}

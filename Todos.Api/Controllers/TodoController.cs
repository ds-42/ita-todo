using AutoMapper;
using Common.Application.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todos.Application.Dto;
using Todos.Application.Features.Todo.Commands.CreateTodo;
using Todos.Application.Features.Todo.Commands.DeleteTodo;
using Todos.Application.Features.Todo.Commands.DoneTodo;
using Todos.Application.Features.Todo.Commands.UpdateTodo;
using Todos.Application.Features.Todo.Queries.GetById;
using Todos.Application.Features.Todo.Queries.GetList;

namespace Todos.Api.Controllers;

[Authorize]
[ApiController]
[Route("todos")]
public class TodoController : BaseController
{
    protected readonly IMapper _mapper;

    public TodoController(IMapper mapper, IMediator mediator) : base(mediator)
    {
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetListQuery getListQuery, CancellationToken cancellationToken = default)
    {
        var todos = await ExecQueryAsync(getListQuery, cancellationToken);
        HttpContext.Response.Headers
            .Append("X-Total-Count", todos.Count.ToString());

        return Ok(todos.Items);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTodo(int id, CancellationToken cancellationToken)
    {
        var item = await ExecQueryAsync(new GetByIdQuery(id), cancellationToken);
        return Ok(item);
    }

    protected IActionResult GetIsDoneState(GetTodoDto item) 
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
        var item = await ExecQueryAsync(new GetByIdQuery(id), cancellationToken);

        return GetIsDoneState(item);
    }


    [HttpPost]
    public async Task<IActionResult> AddTodo(
        CreateTodoCommand createTodoCommand,
        CancellationToken cancellationToken)
    {
        var item = await ExecCommandAsync(createTodoCommand, cancellationToken);

        return Created($"todos/{item.Id}", item);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(
        int id,
        SetTodoDto todo, 
        CancellationToken cancellationToken)
    {
        var updateTodoCommand = _mapper.Map<SetTodoDto, UpdateTodoCommand>(todo);
        updateTodoCommand.Id = id;  

        var item = await ExecCommandAsync(updateTodoCommand, cancellationToken);

        return Ok(item);
    }


    [HttpPatch("{id}/Done")]
    public async Task<IActionResult> DoneTodo(
        int id, 
        CancellationToken cancellationToken)
    {
        var item = await ExecCommandAsync(new DoneTodoCommand() { Id = id }, cancellationToken);

        return GetIsDoneState(item);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteItem([FromBody] int id, CancellationToken cancellationToken)
    {
        await ExecCommandAsync(new DeleteTodoCommand() { Id = id }, cancellationToken);

        return Ok();
    }
}

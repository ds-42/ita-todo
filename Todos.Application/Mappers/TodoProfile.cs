using AutoMapper;
using Common.Domain;
using Todos.Application.Dto;
using Todos.Application.Features.Todo.Commands.CreateTodo;
using Todos.Application.Features.Todo.Commands.UpdateTodo;

namespace Todos.Application.Mappers;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<Todo, GetTodoDto>();
        CreateMap<CreateTodoCommand, Todo>();
        CreateMap<UpdateTodoCommand, Todo>();
        CreateMap<SetTodoDto, UpdateTodoCommand>();
    }
}

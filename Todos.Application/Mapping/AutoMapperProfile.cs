using AutoMapper;
using Common.Domain;
using Todos.Application.Dto;
using Todos.Application.Features.Todo.Commands.CreateTodo;
using Todos.Application.Features.Todo.Commands.DoneTodo;
using Todos.Application.Features.Todo.Commands.UpdateTodo;

namespace Todos.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Todo, GetTodoDto>();
        CreateMap<CreateTodoCommand, Todo>();
        CreateMap<UpdateTodoCommand, Todo>();
        CreateMap<SetTodoDto, UpdateTodoCommand>();
    }
}

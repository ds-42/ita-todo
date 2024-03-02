﻿using AutoMapper;
using Todos.Domain;
using Todos.Services.Dto;

namespace Todos.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<CreateTodoDto, Todo>();
        CreateMap<UpdateTodoDto, Todo>();
    }
}
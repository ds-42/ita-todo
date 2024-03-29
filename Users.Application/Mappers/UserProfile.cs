﻿using AutoMapper;
using Common.Domain.Users;
using Users.Services.Dto;

namespace Users.Application.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<ApplicationUser, GetUserDto>();
    }
}

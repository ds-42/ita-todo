using AutoMapper;
using Common.Domain;
using Users.Services.Dto;

namespace Users.Services.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<User, GetUserDto>();
    }
}

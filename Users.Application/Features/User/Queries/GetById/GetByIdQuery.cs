using MediatR;
using Users.Services.Dto;

namespace Users.Application.Features.User.Queries.GetByIdQuery;

public class GetByIdQuery : IRequest<GetUserDto>
{
    public int Id { get; set; }

    public GetByIdQuery(int id) 
    {
        Id = id;
    }
}

using MediatR;
using Users.Services.Dto;

namespace Users.Application.Features.User.Queries.GetList;

public class GetListQuery : BaseUsersFilter, IRequest<IReadOnlyCollection<GetUserDto>>
{
    public int? offset { get; set; }
    public int? limit { get; set; }
}

using MediatR;
using Users.Services.Dto;

namespace Users.Application.Query.GetList;

public class GetListQuery : BaseUsersFilter, IRequest<IReadOnlyCollection<GetUserDto>>
{
    public int? offset { get; set; }
    public int? limit { get; set; }
}

using MediatR;
using Users.Services.Dto;

namespace Users.Application.Features.User.Queries.GetCount;

public class GetCountQuery : BaseUsersFilter, IRequest<int>
{
}

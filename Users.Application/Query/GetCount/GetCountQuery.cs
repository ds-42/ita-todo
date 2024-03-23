using MediatR;
using Users.Services.Dto;

namespace Users.Application.Query.GetCount;

public class GetCountQuery : BaseUsersFilter, IRequest<int>
{
}

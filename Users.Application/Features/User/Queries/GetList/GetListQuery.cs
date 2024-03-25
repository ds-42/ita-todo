using Common.Application.Dto;
using MediatR;
using Users.Services.Dto;

namespace Users.Application.Features.User.Queries.GetList;

public class GetListQuery : Pagination, IRequest<IReadOnlyCollection<GetUserDto>>
{
}

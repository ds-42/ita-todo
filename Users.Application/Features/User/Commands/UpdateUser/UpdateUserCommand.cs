using MediatR;
using Users.Application.Dto;
using Users.Services.Dto;

namespace Users.Application.Features.User.Commands.UpdateUser;

public class UpdateUserCommand : IRequest<GetUserDto>
{
    public int UserId { get; set; }
    public SetUserDto User { get; set; } = default!;
}



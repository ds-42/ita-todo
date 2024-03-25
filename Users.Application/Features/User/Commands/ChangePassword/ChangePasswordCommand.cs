using MediatR;
using Users.Application.Dto;
using Users.Services.Dto;

namespace Users.Application.Features.User.Commands.UpdateUser;

public class ChangePasswordCommand : IRequest<GetUserDto>
{
    public int UserId { get; set; }
    public string Password { get; set; } = default!;
}



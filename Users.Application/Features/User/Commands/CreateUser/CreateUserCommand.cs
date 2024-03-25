using MediatR;
using Users.Services.Dto;

namespace Users.Application.Features.User.Commands.CreateUser;

public class CreateUserCommand : IRequest<GetUserDto>
{
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
}



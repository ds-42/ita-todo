using MediatR;
using Users.Application.Dto;
using Users.Services.Dto;

namespace Users.Application.Features.User.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public int UserId { get; set; }
}



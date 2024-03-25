using Common.Application.Exceptions;

namespace Users.Application.Exceptions;

public class InvalidUserException : NotFoundException
{
    public InvalidUserException(int userId) : base(new
    {
        Id = userId,
        Message = "Invalid user Id",
    })
    { }
}


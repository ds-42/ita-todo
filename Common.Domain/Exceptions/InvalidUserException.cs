using Common.BL.Exceptions;

namespace Common.Domain.Exceptions;

public class InvalidUserException : NotFoundException
{
    public InvalidUserException(int userId) : base(new 
    { 
        Id = userId,
        Message = "Invalid user Id",
    }) { }
}


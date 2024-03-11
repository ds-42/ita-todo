using Common.Api.Exceptions;
using Common.Domain;
using Common.Domain.Exceptions;

namespace Common.Domain.Exceptions
{
    public class InvalidUserException : NotFoundException
    {
        public InvalidUserException(int userId) : base(new 
        { 
            Id = userId,
            Message = "Invalid user Id",
        }) { }
    }
}

public abstract partial class Exceptions
{
    public static InvalidUserException InvalidUser(int userId)
        => new InvalidUserException(userId);
}
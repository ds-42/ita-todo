using System.Net;

namespace Common.Application.Exceptions;

public class ForbiddenException : HttpException
{
    public ForbiddenException() : base(new { Message = "Forbidden" }, HttpStatusCode.Forbidden)
    {
    }
}


using System.Net;

namespace Common.BL.Exceptions;

public class ForbiddenException : HttpException
{
    public ForbiddenException() : base(new { Message = "Forbidden" }, HttpStatusCode.Forbidden)
    {
    }
}


using System.Net;

namespace Common.BL.Exceptions;

public class ForbiddenException : HttpException
{
    public ForbiddenException(string error) : base(error, HttpStatusCode.Forbidden)
    {
    }
}


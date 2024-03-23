using System.Net;

namespace Common.Application.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string error) : base(error, HttpStatusCode.BadRequest)
    {
    }
}


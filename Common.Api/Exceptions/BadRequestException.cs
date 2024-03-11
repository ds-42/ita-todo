using System.Net;

namespace Common.Api.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string error) : base(error, HttpStatusCode.BadRequest) 
    {
    }
}


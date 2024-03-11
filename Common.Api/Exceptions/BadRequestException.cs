using System.Net;

namespace Common.Api.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string error) : base(error, HttpStatusCode.BadRequest) 
    {
    }
}

public abstract partial class Exceptions
{
    public static BadRequestException BadRequest(string error)
        => new BadRequestException(error);
}
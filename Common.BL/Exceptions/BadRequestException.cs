using System.Net;

namespace Common.BL.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string error) : base(error, HttpStatusCode.BadRequest) 
    {
    }
}


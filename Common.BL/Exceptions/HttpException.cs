using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Common.BL.Exceptions;

public class HttpException : Exception
{
    public readonly HttpStatusCode StatusCode;

    public HttpException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
    { 
        StatusCode = statusCode;
    }

    public HttpException(object item, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) :
        this(JsonSerializer.Serialize(item), statusCode)
    { 
    }

    public virtual async Task WriteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)StatusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(Message);
    }
}

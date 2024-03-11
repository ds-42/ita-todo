using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api.Exceptions;

public class HttpException : Exception
{
    public readonly HttpStatusCode StatusCode;

    public HttpException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
    { 
        StatusCode = statusCode;
    }



    public virtual async Task WriteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)StatusCode;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(Message);
    }
}

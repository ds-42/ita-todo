using Common.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Common.Api;

public class ExceptionsHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionsHandlerMiddleware(RequestDelegate next) 
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            switch (e)
            {
                case HttpException httpException:
                    await httpException.WriteAsync(httpContext);
                    break;

                default:
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(e.Message);
                    break;
            }
        }
    }
}

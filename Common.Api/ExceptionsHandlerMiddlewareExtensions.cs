using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Api;

public static class ExceptionsHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionsHandlerMiddleware>();
        return app;
    }
}

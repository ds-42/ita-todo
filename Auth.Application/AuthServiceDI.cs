using Common.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class AuthServiceDI
{
    public static IServiceCollection AddAuthApplication(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();

        return services;
    }
}


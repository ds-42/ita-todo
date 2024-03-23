using Microsoft.Extensions.DependencyInjection;
namespace Common.Api;

public static class CommonDI
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        return services;
    }
}

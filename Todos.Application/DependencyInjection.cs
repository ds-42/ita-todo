using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Todos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTodoApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSingleton<TodosMemoryCache>();

        services.AddValidatorsFromAssemblies(
            new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}


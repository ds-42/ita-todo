using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Todos.Application.Mapping;

namespace Todos.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddTodoApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddSingleton<TodosMemoryCache>();

        services.AddValidatorsFromAssemblies(
            new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}


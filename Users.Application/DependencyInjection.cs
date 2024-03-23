using Common.Application.Abstractions.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Application.Command.CreateUser;
using Users.Application.Query.GetCount;
using Users.Application.Query.GetList;
using Users.Services.Mapping;

namespace Users.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddUserApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        services.AddSingleton<UsersMemoryCache>();

        services.AddValidatorsFromAssemblies(
            new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        return services;
    }
}

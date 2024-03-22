using Common.Domain;
using Common.Repositiories;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Users.Services.Command.CreateUser;
using Users.Services.Mapping;
using Users.Services.Query.GetCount;
using Users.Services.Query.GetList;

namespace Users.Services;

public static class UsersDI
{
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));

        services.AddTransient<IRepository<RefreshToken>, SqlServerBaseRepository<RefreshToken>>();
        services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();
        services.AddTransient<IRepository<ApplicationUserRole>, SqlServerBaseRepository<ApplicationUserRole>>();
        services.AddTransient<IRepository<ApplicationUserApplicationRole>, SqlServerBaseRepository<ApplicationUserApplicationRole>>();

        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAuthService, AuthService>();

        services.AddSingleton<UsersMemoryCache>();

        services.AddTransient<CreateUserCommandHandler>();
        services.AddTransient<GetListQueryHandler>();
        services.AddTransient<GetCountQueryHandler>();

        services.AddValidatorsFromAssemblies(
            new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

        return services;
    }

}

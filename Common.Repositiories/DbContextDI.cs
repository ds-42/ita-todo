using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Repositiories
{
    public static class DbContextDI
    {
        public static IServiceCollection AddTodoDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext, ApplicationDbContext>(
                options => 
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
            );

            return services;
        }
    }
}

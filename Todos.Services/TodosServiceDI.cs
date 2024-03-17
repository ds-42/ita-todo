using Common.Domain;
using Common.Repositiories;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Todos.Domain;
using Todos.Services.Mapping;

namespace Todos.Services
{
    public static class TodosServiceDI
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services) 
        {
            services.AddTransient<IRepository<Todo>, SqlServerBaseRepository<Todo>>();
            services.AddTransient<IRepository<ApplicationUser>, SqlServerBaseRepository<ApplicationUser>>();

            services.AddTransient<ITodoService, TodoService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddValidatorsFromAssemblies(
                new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            return services;
        }
    }
}

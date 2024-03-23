using Common.Domain;
using Common.Persistence;
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
            services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
            services.AddTransient<IRepository<ApplicationUser>, BaseRepository<ApplicationUser>>();

            services.AddTransient<ITodoService, TodoService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddValidatorsFromAssemblies(
                new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            return services;
        }
    }
}

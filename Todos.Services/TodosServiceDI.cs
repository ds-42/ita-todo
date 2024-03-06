using Common.Domain;
using Common.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Todos.Domain;
using Todos.Services.Dto;
using Todos.Services.Mapping;
using Todos.Services.Validators;

namespace Todos.Services
{
    public static class TodosServiceDI
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services) 
        {
            services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
            services.AddTransient<IRepository<User>, BaseRepository<User>>();

            services.AddTransient<ITodoService, TodoService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            services.AddValidatorsFromAssemblies(
                new[] { Assembly.GetExecutingAssembly() }, includeInternalTypes: true);

            return services;
        }
    }
}

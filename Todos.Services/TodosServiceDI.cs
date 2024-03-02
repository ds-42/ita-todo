using Common.Domain;
using Common.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Todos.Domain;
using Todos.Services.Mapping;

namespace Todos.Services
{
    public static class TodosServiceDI
    {
        public static IServiceCollection AddTodoServices(this IServiceCollection services) 
        {
            services.AddTransient<IRepository<Todo>, BaseRepository<Todo>>();
            services.AddTransient<IRepository<User>, BaseRepository<User>>();
            // faq: Что будет если добавить два IUserRepository?
            // faq: Правильно ли IUserRepository добавлять здесь, ведь мы не знаем какая реализация репозитория будет?

            services.AddTransient<ITodoService, TodoService>();

            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }
    }
}

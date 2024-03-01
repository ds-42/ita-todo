using Microsoft.Extensions.DependencyInjection;
using Todos.Services.Mapping;

namespace Todos.Services
{
    public static class TodosServiceDI
    {
        public static IServiceCollection AddAutoMapperTodo(this IServiceCollection services) 
            => services.AddAutoMapper(typeof(AutoMapperProfile));
    }
}

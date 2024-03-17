using Common.Domain;
using Common.Repositories;
using Todos.Domain;

namespace Todos.Api.Repositories;

public static class TodoUserRepository
{
    public static async Task UploadUserData(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var userRepository = serviceProvider.GetService<IRepository<ApplicationUser>>()!;

        async Task<ApplicationUser> AddItem(string name)
        {
            var item = new ApplicationUser()
            {
                Login = name,
            };

            await userRepository.AddAsync(item);

            return item;
        }

        if (await userRepository.CountAsync() > 0)
            return;

        await AddItem("user-1");
        await AddItem("user-2");
        await AddItem("user-3");
    }
}

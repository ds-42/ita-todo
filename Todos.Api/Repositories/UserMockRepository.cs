using Common.Domain;
using Common.Repositories;
using Todos.Domain;

namespace Todos.Api.Repositories;

public static class TodoUserRepository
{
    public static void UploadUserData(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var userRepository = serviceProvider.GetService<IRepository<User>>()!;

        User AddItem(string name)
        {
            var item = new User()
            {
                Name = name,
            };

            userRepository.Add(item);

            return item;
        }

        if (userRepository.Count() > 0)
            return;

        AddItem("user-1");
        AddItem("user-2");
        AddItem("user-3");
    }
}

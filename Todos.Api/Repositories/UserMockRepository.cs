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

        User AddItem(int id, string name)
        {
            var item = new User()
            {
                Id = id,
                Name = name,
            };

            userRepository.Add(item);

            return item;
        }


        AddItem(1, "user-1");
        AddItem(2, "user-2");
        AddItem(3, "user-3");
    }
}

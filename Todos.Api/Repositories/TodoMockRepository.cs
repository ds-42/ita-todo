using Common.Application.Abstractions.Persistence;
using Common.Domain;

namespace Todos.Api.Repositories;

public static class TodoMockRepository
{
    public static async Task UploadTodoData(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var todoRepository = serviceProvider.GetService<IRepository<Todo>>()!;

        async Task<Todo> AddItem(string label, int ownerId)
        {
            var item = new Todo()
            {
                OwnerId = ownerId,
                IsDone = false,
                Label = label,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            await todoRepository.AddAsync(item);

            return item;
        }

        if (await todoRepository.CountAsync() > 0)
            return;

        await AddItem("task-1", 1);
        await AddItem("task-2", 2);
        await AddItem("task-3", 2);
        await AddItem("task-4", 3);
        await AddItem("task-5", 1);
    }
}

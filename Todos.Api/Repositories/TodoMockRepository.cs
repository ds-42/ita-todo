using Common.Repositories;
using Todos.Domain;

namespace Todos.Api.Repositories;

public static class TodoMockRepository
{
    public static void UploadTodoData(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var todoRepository = serviceProvider.GetService<IRepository<Todo>>()!;

        Todo AddItem(int id, string label, int ownerId)
        {
            var item = new Todo()
            {
                Id = id,
                OwnerId = ownerId,
                IsDone = false,
                Label = label,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            todoRepository.Add(item);

            return item;
        }


        AddItem(1, "task-1", 1);
        AddItem(2, "task-2", 2);
        AddItem(3, "task-3", 2);
        AddItem(4, "task-4", 3);
        AddItem(5, "task-5", 1);
    }
}

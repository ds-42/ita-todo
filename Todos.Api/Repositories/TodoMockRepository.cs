using Common.Repositories;
using Todos.Domain;

namespace Todos.Api.Repositories;

public static class TodoMockRepository
{
    public static void UploadTodoData(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var todoRepository = serviceProvider.GetService<IRepository<Todo>>()!;

        Todo AddItem(string label, int ownerId)
        {
            var item = new Todo()
            {
                OwnerId = ownerId,
                IsDone = false,
                Label = label,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            todoRepository.Add(item);

            return item;
        }

        if (todoRepository.Count() > 0)
            return;

        AddItem("task-1", 1);
        AddItem("task-2", 2);
        AddItem("task-3", 2);
        AddItem("task-4", 3);
        AddItem("task-5", 1);
    }
}

using Todos.Domain;
using Todos.Repositiories;

namespace Todos.Api.Repositories;

public class TodoMockRepository : TodoRepository
{
    public static void LoadData()
    {
        Todo AddItem(string label, int ownerId)
        {
            var item = new Todo()
            {
                Id = Items.Count + 1,
                OwnerId = ownerId,
                IsDone = false,
                Label = label,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            Items.Add(item);

            return item;
        }


        AddItem("task-1", 1);
        AddItem("task-2", 2);
        AddItem("task-3", 2);
        AddItem("task-4", 3);
        AddItem("task-5", 1);
    }

}

using ToDo.Models;

namespace ToDo.Apis
{
    public static class TodoApi
    {
        public static List<Todo> Items = new();

        public static void AppendMockData() 
        {
            Add("task-1");
            Add("task-2");
            Add("task-3");
            Add("task-4");
            Add("task-5");
        }

        public static List<Todo> GetItems(int limit, int offset) 
        {
            return Items
                .OrderBy(t => t.Id)
                .Skip(offset)
                .Take(limit)
                .ToList();
        }

        public static Todo? Find(int id) 
        {
            return Items
                .FirstOrDefault(t => t.Id == id);
        }

        public static Todo Add(string label) 
        {
            var item = new Todo()
            {
                Id = Items.Count + 1,
                IsDone = false,
                Label = label,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
            };

            Items.Add(item);

            return item;
        }

        public static Todo? Update(int id, string label)
        {
            var item = Find(id);

            if (item != null)
            {
                item.Label = label;
                item.UpdateDate = DateTime.UtcNow;
            }

            return item;
        }


        public static Todo? Done(int id)
        {
            var item = Find(id);

            if (item != null)
            {
                item.IsDone = true;
                item.UpdateDate = DateTime.UtcNow;
            }

            return item;
        }

        public static Todo? Delete(int id)
        {
            var item = Find(id);

            if (item != null)
            {
                Items.Remove(item);
            }

            return item;
        }

    }
}

using Common.Domain;
using Common.Repositories;

namespace Todos.Api.Repositories
{
    public class UserMockRepository : UserRepository
    {
        public static void LoadData()
        {
            User AddItem(string name)
            {
                var item = new User()
                {
                    Id = Items.Count + 1,
                    Name = name,
                };

                Items.Add(item);

                return item;
            }


            AddItem("user-1");
            AddItem("user-2");
            AddItem("user-3");
        }
    }
}

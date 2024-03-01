using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Services.Dto
{
    public class UpdateTodoDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Label { get; set; } = default!;
        public bool IsDone { get; set; }
    }
}

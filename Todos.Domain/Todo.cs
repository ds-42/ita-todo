using Common.Domain;

namespace Todos.Domain;

public class Todo
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public User Owner { get; set; } = default!;
    public string Label { get; set; } = default!;
    public bool IsDone { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }

    public static Todo Create(int id, string label) 
    {
        return new Todo 
        { 
            Id = id, 
            Label = label,
            IsDone = false,
            CreateDate = DateTime.UtcNow,
            UpdateDate = DateTime.UtcNow,
        };
    }
}

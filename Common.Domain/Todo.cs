using Common.Domain.Users;

namespace Common.Domain;

public class Todo
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public ApplicationUser Owner { get; set; } = default!;
    public string Label { get; set; } = default!;
    public bool IsDone { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
}

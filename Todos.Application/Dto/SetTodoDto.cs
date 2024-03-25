namespace Todos.Application.Dto;

public class SetTodoDto
{
    public int OwnerId { get; set; }
    public string Label { get; set; } = default!;
    public bool IsDone { get; set; }
}

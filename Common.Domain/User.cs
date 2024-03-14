namespace Common.Domain;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int RoleId { get; set; }
    public UserRole Role { get; set; } = default!;
    //    public ICollection<Todo> Todos { get; set; } = default!;

}

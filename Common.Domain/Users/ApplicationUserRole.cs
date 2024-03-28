namespace Common.Domain.Users;

public class ApplicationUserRole
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public ICollection<ApplicationUserApplicationRole> Users { get; set; } = default!;
}

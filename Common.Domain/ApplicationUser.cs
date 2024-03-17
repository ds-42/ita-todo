namespace Common.Domain;

public class ApplicationUser
{
    public int Id { get; set; }
    public string Login { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int RoleId { get; set; }
    public IEnumerable<ApplicationUserApplicationRole> Roles { get; set; } = default!;

}

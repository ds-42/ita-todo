using Microsoft.EntityFrameworkCore;
using Common.Domain;
using Common.Domain.Auth;
using Common.Domain.Users;
using System.Reflection;

namespace Common.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
    public DbSet<ApplicationUserApplicationRole> ApplicationUserApplicationRole { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}

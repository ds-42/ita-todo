using Microsoft.EntityFrameworkCore;
using Common.Domain;

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
        modelBuilder.Entity<Todo>().HasKey(t => t.Id);
        modelBuilder.Entity<Todo>().Property(t => t.Label).HasMaxLength(100).IsRequired();

        modelBuilder.Entity<Todo>().Navigation(t => t.Owner).AutoInclude();
        modelBuilder.Entity<Todo>().HasOne(t => t.Owner)
            //            .WithMany(t => t.Todos)
            .WithMany()
            .HasForeignKey(t => t.OwnerId);

        modelBuilder.Entity<ApplicationUser>().HasKey(t => t.Id);
        modelBuilder.Entity<ApplicationUser>().Property(t => t.Login).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<ApplicationUser>().HasIndex(t => t.Login).IsUnique();
        modelBuilder.Entity<ApplicationUser>().Navigation(t => t.Roles).AutoInclude();

        modelBuilder.Entity<RefreshToken>().HasKey(t => t.Id);
        modelBuilder.Entity<RefreshToken>().Property(t => t.Id).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<RefreshToken>().HasOne(t => t.ApplicationUser)
            .WithMany()
            .HasForeignKey(t => t.ApplicationUserId);

        modelBuilder.Entity<ApplicationUserApplicationRole>().HasKey(t =>
            new { t.ApplicationUserId, t.ApplicationUserRoleId });
        modelBuilder.Entity<ApplicationUserApplicationRole>().Navigation(t => t.ApplicationUserRole).AutoInclude();

        modelBuilder.Entity<ApplicationUser>().HasMany(t => t.Roles)
            .WithOne(t => t.ApplicationUser)
            .HasForeignKey(t => t.ApplicationUserId);

        modelBuilder.Entity<ApplicationUserRole>().HasMany(t => t.Users)
            .WithOne(t => t.ApplicationUserRole)
            .HasForeignKey(t => t.ApplicationUserRoleId);

        modelBuilder.Entity<ApplicationUserRole>().HasKey(t => t.Id);
        modelBuilder.Entity<ApplicationUserRole>().Property(t => t.Name).HasMaxLength(50).IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}

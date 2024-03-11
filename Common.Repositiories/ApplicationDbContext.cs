using Microsoft.EntityFrameworkCore;
using Common.Domain;
using Todos.Domain;

namespace Common.Repositiories;

public class ApplicationDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>().HasKey(t => t.Id);
        modelBuilder.Entity<Todo>().Property(t => t.Label).HasMaxLength(100).IsRequired();

        modelBuilder.Entity<Todo>().HasOne(t => t.Owner)
//            .WithMany(t => t.Todos)
            .WithMany()
            .HasForeignKey(t => t.OwnerId);

        modelBuilder.Entity<User>().HasKey(t => t.Id);
        modelBuilder.Entity<User>().Property(t => t.Name).HasMaxLength(50).IsRequired();


        base.OnModelCreating(modelBuilder);
    }
}

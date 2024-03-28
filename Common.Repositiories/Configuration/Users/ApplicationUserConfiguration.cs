using Common.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Persistence.Configuration.Users;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Login).HasMaxLength(50).IsRequired();
        builder.HasIndex(t => t.Login).IsUnique();
        builder.Navigation(t => t.Roles).AutoInclude();

        builder.HasMany(t => t.Roles)
            .WithOne(t => t.ApplicationUser)
            .HasForeignKey(t => t.ApplicationUserId);
    }
}

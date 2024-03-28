using Common.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Persistence.Configuration.Users;

public class ApplicationUserApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationUserApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationUserApplicationRole> builder)
    {
        builder.HasKey(t =>
            new { t.ApplicationUserId, t.ApplicationUserRoleId });
        builder.Navigation(t => t.ApplicationUserRole).AutoInclude();
    }
}

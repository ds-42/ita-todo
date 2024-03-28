using Common.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common.Persistence.Configuration;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Label).HasMaxLength(100).IsRequired();

        builder.Navigation(t => t.Owner).AutoInclude();
        builder.HasOne(t => t.Owner)
            //            .WithMany(t => t.Todos)
            .WithMany()
            .HasForeignKey(t => t.OwnerId);
    }
}

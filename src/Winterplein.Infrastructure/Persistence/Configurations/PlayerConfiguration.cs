using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

        builder.OwnsOne(p => p.Name, name =>
        {
            name.Property(n => n.FirstName).HasColumnName("FirstName").IsRequired();
            name.Property(n => n.LastName).HasColumnName("LastName").IsRequired();
        });

        builder.Property(p => p.Gender)
            .HasConversion<string>();
    }
}

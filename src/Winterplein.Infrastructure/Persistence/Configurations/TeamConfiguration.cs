using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Persistence.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.HasOne(t => t.Player1)
            .WithMany()
            .HasForeignKey("Player1Id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Player2)
            .WithMany()
            .HasForeignKey("Player2Id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.HasOne(m => m.Team1)
            .WithMany()
            .HasForeignKey("Team1Id")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Team2)
            .WithMany()
            .HasForeignKey("Team2Id")
            .OnDelete(DeleteBehavior.Restrict);
    }
}

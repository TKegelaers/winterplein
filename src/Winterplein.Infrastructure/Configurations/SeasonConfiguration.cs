using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Configurations;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedOnAdd();
        builder.Property(s => s.Name).IsRequired();

        builder.Property(s => s.StartDate).HasColumnType("date");
        builder.Property(s => s.EndDate).HasColumnType("date");
        builder.Property(s => s.StartHour).HasColumnType("time");
        builder.Property(s => s.EndHour).HasColumnType("time");

        builder.HasMany(s => s.Players)
            .WithMany()
            .UsingEntity("SeasonPlayers");

        builder.Navigation(s => s.Players)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

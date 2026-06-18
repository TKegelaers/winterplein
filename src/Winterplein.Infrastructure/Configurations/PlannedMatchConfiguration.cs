using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Configurations;

public class PlannedMatchConfiguration : IEntityTypeConfiguration<PlannedMatch>
{
    public void Configure(EntityTypeBuilder<PlannedMatch> builder)
    {
        builder.HasKey(pm => pm.Id);
        builder.Property(pm => pm.Id).ValueGeneratedOnAdd();

        builder.Property(pm => pm.SeasonId);
        builder.Property(pm => pm.Date).HasColumnType("date");

        ConfigureTeam(builder, pm => pm.Team1, "Team1");
        ConfigureTeam(builder, pm => pm.Team2, "Team2");
    }

    private static void ConfigureTeam(
        EntityTypeBuilder<PlannedMatch> builder,
        System.Linq.Expressions.Expression<Func<PlannedMatch, PlannedTeam?>> teamSelector,
        string teamPrefix)
    {
        builder.OwnsOne(teamSelector, team =>
        {
            ConfigurePlayer(team, t => t.Player1, $"{teamPrefix}Player1");
            ConfigurePlayer(team, t => t.Player2, $"{teamPrefix}Player2");
        });

        builder.Navigation(teamSelector).IsRequired();
    }

    private static void ConfigurePlayer(
        OwnedNavigationBuilder<PlannedMatch, PlannedTeam> team,
        System.Linq.Expressions.Expression<Func<PlannedTeam, PlannedPlayer?>> playerSelector,
        string playerPrefix)
    {
        team.OwnsOne(playerSelector, player =>
        {
            player.Property(p => p.PlayerId).HasColumnName($"{playerPrefix}PlayerId");
            player.Property(p => p.FirstName).HasColumnName($"{playerPrefix}FirstName").IsRequired();
            player.Property(p => p.LastName).HasColumnName($"{playerPrefix}LastName").IsRequired();
            player.Property(p => p.Gender).HasColumnName($"{playerPrefix}Gender").HasConversion<string>().IsRequired();
        });

        team.Navigation(playerSelector).IsRequired();
    }
}

using Microsoft.EntityFrameworkCore;
using Winterplein.Domain.Entities;

namespace Winterplein.Infrastructure.Persistence;

public class WinterpleinDbContext : DbContext
{
    public WinterpleinDbContext(DbContextOptions<WinterpleinDbContext> options) : base(options) { }

    public DbSet<Player> Players => Set<Player>();
    public DbSet<Season> Seasons => Set<Season>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WinterpleinDbContext).Assembly);
    }
}

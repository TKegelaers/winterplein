using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Infrastructure;

namespace Winterplein.IntegrationTests.SeedBuilders;

/// <summary>
/// Fluent builder that persists a <see cref="Player"/> through
/// <see cref="WinterpleinDbContext"/>. Construct via the public constructor with
/// Id = 0 so SQL Server identity assigns the generated Id, which flows back on the
/// returned model after <see cref="Seed"/>.
/// </summary>
public class PlayerSeedBuilder
{
    private string _firstName = "John";
    private string _lastName = "Doe";
    private Gender _gender = Gender.Male;

    public PlayerSeedBuilder WithFirstName(string firstName) { _firstName = firstName; return this; }
    public PlayerSeedBuilder WithLastName(string lastName) { _lastName = lastName; return this; }
    public PlayerSeedBuilder WithGender(Gender gender) { _gender = gender; return this; }

    public async Task<Player> Seed(WinterpleinDbContext dbContext)
    {
        var player = new Player(0, new Name(_firstName, _lastName), _gender);
        dbContext.Add(player);
        await dbContext.SaveChangesAsync();
        return player;
    }
}

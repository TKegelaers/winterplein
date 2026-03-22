using Winterplein.Domain.Entities;

namespace Winterplein.UnitTests.Common.Builders;

public class TeamBuilder
{
    private int _id = 1;
    private Player _player1 = new PlayerBuilder().WithId(1).Build();
    private Player _player2 = new PlayerBuilder().WithId(2).Build();

    public TeamBuilder WithId(int id) { _id = id; return this; }
    public TeamBuilder WithPlayer1(Player player1) { _player1 = player1; return this; }
    public TeamBuilder WithPlayer2(Player player2) { _player2 = player2; return this; }

    public Team Build() => new(_id, _player1, _player2);
}

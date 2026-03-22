using Winterplein.Domain.Entities;

namespace Winterplein.Application.Interfaces;

public interface IPlayerRepository
{
    IReadOnlyList<Player> GetAll();
    void Add(Player player);
    void Remove(Guid id);
}

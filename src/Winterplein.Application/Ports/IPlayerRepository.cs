using Winterplein.Domain.Entities;

namespace Winterplein.Application.Ports;

public interface IPlayerRepository
{
    Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken ct = default);
    Task<Player?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<Player> AddAsync(Player player, CancellationToken ct = default);
    Task RemoveAsync(int id, CancellationToken ct = default);
}

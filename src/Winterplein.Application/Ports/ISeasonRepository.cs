using Winterplein.Domain.Entities;

namespace Winterplein.Application.Ports;

public interface ISeasonRepository
{
    Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default);
    Task<Season?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Season> AddAsync(Season season, CancellationToken ct = default);
    Task<Season> UpdateAsync(Season season, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

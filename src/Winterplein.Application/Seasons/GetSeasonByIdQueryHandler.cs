using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class GetSeasonByIdQueryHandler
{
    public static async Task<Season?> Handle(GetSeasonByIdQuery query, ISeasonRepository seasonRepository, CancellationToken ct = default) =>
        await seasonRepository.GetByIdAsync(query.Id, ct);
}

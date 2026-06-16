using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.QueryHandlers.GetSeasonById;

public static class GetSeasonByIdQueryHandler
{
    public static async Task<Season?> Handle(GetSeasonByIdQuery query, ISeasonRepository seasonRepository, CancellationToken ct = default) =>
        await seasonRepository.GetByIdAsync(query.Id, ct);
}

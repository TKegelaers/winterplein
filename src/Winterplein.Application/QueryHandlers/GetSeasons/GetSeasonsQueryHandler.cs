using Winterplein.Application.Ports;
using Winterplein.Application.IO.Queries;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.QueryHandlers.GetSeasons;

public static class GetSeasonsQueryHandler
{
    public static async Task<List<Season>> Handle(GetSeasonsQuery query, ISeasonRepository seasonRepository, CancellationToken ct = default) =>
        (await seasonRepository.GetAllAsync(ct)).ToList();
}

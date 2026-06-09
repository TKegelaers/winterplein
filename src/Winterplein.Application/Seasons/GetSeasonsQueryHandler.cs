using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class GetSeasonsQueryHandler
{
    public static async Task<List<Season>> Handle(GetSeasonsQuery query, ISeasonRepository seasonRepository, CancellationToken ct = default) =>
        (await seasonRepository.GetAllAsync(ct)).ToList();
}

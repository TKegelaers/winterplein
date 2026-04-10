using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class GetSeasonsQueryHandler
{
    public static List<Season> Handle(GetSeasonsQuery query, ISeasonRepository seasonRepository) =>
        seasonRepository.GetAll().ToList();
}

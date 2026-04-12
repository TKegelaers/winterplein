using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class GetSeasonByIdQueryHandler
{
    public static Season? Handle(GetSeasonByIdQuery query, ISeasonRepository seasonRepository) =>
        seasonRepository.GetById(query.Id);
}

using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public static class UpdateSeasonCommandHandler
{
    public static Season Handle(UpdateSeasonCommand request, ISeasonRepository seasonRepository)
    {
        var existing = seasonRepository.GetById(request.Id)
            ?? throw new KeyNotFoundException($"Season {request.Id} not found.");

        var updated = new Season(request.Id, request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour, existing.Players);
        seasonRepository.Update(updated);
        return updated;
    }
}

using Winterplein.Domain.Entities;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Mappers;

public static class SeasonMapper
{
    public static SeasonDto ToDto(this Season season)
    {
        var matchdays = season.GetMatchdays().ToList();
        return new SeasonDto(
            season.Id,
            season.Name,
            season.StartDate,
            season.EndDate,
            season.Weekday,
            season.StartHour,
            season.EndHour,
            matchdays,
            matchdays.Count,
            season.Players.Select(p => p.ToDto()).ToList());
    }
}

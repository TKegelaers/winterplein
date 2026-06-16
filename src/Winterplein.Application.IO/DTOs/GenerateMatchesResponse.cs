namespace Winterplein.Application.IO.DTOs;

public record GenerateMatchesResponse(List<MatchDto> Matches, int TotalCount);

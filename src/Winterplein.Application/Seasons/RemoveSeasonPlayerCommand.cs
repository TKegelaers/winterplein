using MediatR;

namespace Winterplein.Application.Seasons;

public record RemoveSeasonPlayerCommand(int SeasonId, int PlayerId) : IRequest<bool>;

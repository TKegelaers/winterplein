using MediatR;
using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public class RemoveSeasonPlayerCommandHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<RemoveSeasonPlayerCommand, bool>
{
    public Task<bool> Handle(RemoveSeasonPlayerCommand request, CancellationToken cancellationToken)
    {
        var season = seasonRepository.GetById(request.SeasonId);
        if (season == null)
            return Task.FromResult(false);

        season.RemovePlayer(request.PlayerId);
        seasonRepository.Update(season);
        return Task.FromResult(true);
    }
}

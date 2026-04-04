using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public class UpdateSeasonCommandHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<UpdateSeasonCommand, bool>
{
    public Task<bool> Handle(UpdateSeasonCommand request, CancellationToken cancellationToken)
    {
        var existing = seasonRepository.GetById(request.Id);
        if (existing == null)
            return Task.FromResult(false);

        var updated = new Season(request.Id, request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour, existing.Players);
        return Task.FromResult(seasonRepository.Update(updated));
    }
}

using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public class CreateSeasonCommandHandler(ISeasonRepository seasonRepository)
    : IRequestHandler<CreateSeasonCommand, int>
{
    public Task<int> Handle(CreateSeasonCommand request, CancellationToken cancellationToken)
    {
        var season = new Season(0, request.Name, request.StartDate, request.EndDate,
            request.Weekday, request.StartHour, request.EndHour);
        var created = seasonRepository.Add(season);
        return Task.FromResult(created.Id);
    }
}

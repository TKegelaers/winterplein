using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public static class DeleteSeasonCommandHandler
{
    public static void Handle(DeleteSeasonCommand request, ISeasonRepository seasonRepository)
    {
        var deleted = seasonRepository.Delete(request.Id);
        if (!deleted)
            throw new KeyNotFoundException($"Season {request.Id} not found.");
    }
}

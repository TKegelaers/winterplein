using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public static class DeleteSeasonCommandHandler
{
    public static void Handle(DeleteSeasonCommand command, ISeasonRepository seasonRepository)
    {
        var deleted = seasonRepository.Delete(command.Id);
        if (!deleted)
            throw new KeyNotFoundException($"Season {command.Id} not found.");
    }
}

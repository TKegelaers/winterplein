using Winterplein.Application.Interfaces;

namespace Winterplein.Application.Seasons;

public static class DeleteSeasonCommandHandler
{
    public static async Task Handle(DeleteSeasonCommand command, ISeasonRepository seasonRepository, CancellationToken ct = default)
    {
        await seasonRepository.DeleteAsync(command.Id, ct);
    }
}

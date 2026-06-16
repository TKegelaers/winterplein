using Winterplein.Application.Ports;
using Winterplein.Application.IO.Commands;

namespace Winterplein.Application.CommandHandlers.DeleteSeason;

public static class DeleteSeasonCommandHandler
{
    public static async Task Handle(DeleteSeasonCommand command, ISeasonRepository seasonRepository, CancellationToken ct = default)
    {
        await seasonRepository.DeleteAsync(command.Id, ct);
    }
}

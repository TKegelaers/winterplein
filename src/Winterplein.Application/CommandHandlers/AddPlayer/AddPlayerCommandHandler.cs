using Winterplein.Application.Ports;
using Winterplein.Application.Mappers;
using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Application.IO.Commands;
using Winterplein.Application.IO.DTOs;

namespace Winterplein.Application.CommandHandlers.AddPlayer;

public static class AddPlayerCommandHandler
{
    public static async Task<PlayerDto> Handle(AddPlayerCommand command, IPlayerRepository repo, CancellationToken ct = default)
    {
        var gender = (Gender)command.Gender;
        var name = new Name(command.FirstName, command.LastName);
        var player = await repo.AddAsync(new Player(0, name, gender), ct);
        return player.ToDto();
    }
}

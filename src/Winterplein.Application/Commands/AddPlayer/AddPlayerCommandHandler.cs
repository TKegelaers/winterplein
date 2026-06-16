using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Domain.Entities;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.AddPlayer;

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

using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.AddPlayer;

public static class AddPlayerCommandHandler
{
    public static PlayerDto Handle(AddPlayerCommand command, IPlayerRepository repo)
    {
        var gender = (Gender)command.Gender;
        var name = new Name(command.FirstName, command.LastName);
        var player = repo.Add(name, gender);
        return player.ToDto();
    }
}

using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.AddPlayer;

public static class AddPlayerCommandHandler
{
    public static PlayerDto Handle(AddPlayerCommand request, IPlayerRepository repo)
    {
        var gender = (Gender)request.Gender;
        var name = new Name(request.FirstName, request.LastName);
        var player = repo.Add(name, gender);
        return player.ToDto();
    }
}

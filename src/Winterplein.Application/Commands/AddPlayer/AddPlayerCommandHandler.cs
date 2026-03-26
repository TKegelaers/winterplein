using MediatR;
using Winterplein.Application.Interfaces;
using Winterplein.Application.Mappers;
using Winterplein.Domain.Enums;
using Winterplein.Domain.ValueObjects;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.AddPlayer;

public class AddPlayerCommandHandler(IPlayerRepository repo)
    : IRequestHandler<AddPlayerCommand, PlayerDto>
{
    public Task<PlayerDto> Handle(AddPlayerCommand request, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<Gender>(request.Gender, ignoreCase: true, out var gender))
            throw new ArgumentException($"Invalid gender '{request.Gender}'. Valid values: Male, Female.");

        var name = new Name(request.FirstName, request.LastName);
        var player = repo.Add(name, gender);
        return Task.FromResult(player.ToDto());
    }
}

using MediatR;

namespace Winterplein.Application.Commands.RemovePlayer;

public record RemovePlayerCommand(int Id) : IRequest;

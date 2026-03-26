using MediatR;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Queries.GetAllPlayers;

public record GetAllPlayersQuery : IRequest<List<PlayerDto>>;

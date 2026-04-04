using MediatR;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Seasons;

public record GetSeasonPlayersQuery(int SeasonId) : IRequest<List<PlayerDto>?>;

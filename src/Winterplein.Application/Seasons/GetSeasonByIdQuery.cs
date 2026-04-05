using MediatR;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public record GetSeasonByIdQuery(int Id) : IRequest<Season?>;

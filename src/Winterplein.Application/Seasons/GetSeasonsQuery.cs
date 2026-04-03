using MediatR;
using Winterplein.Domain.Entities;

namespace Winterplein.Application.Seasons;

public record GetSeasonsQuery : IRequest<List<Season>>;

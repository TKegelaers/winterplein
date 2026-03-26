using MediatR;

namespace Winterplein.Application.Queries.GetMatchCount;

public record GetMatchCountQuery : IRequest<int>;

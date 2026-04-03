using MediatR;

namespace Winterplein.Application.Seasons;

public record DeleteSeasonCommand(int Id) : IRequest<bool>;

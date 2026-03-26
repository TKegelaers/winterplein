using MediatR;
using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.GenerateMatches;

public record GenerateMatchesCommand : IRequest<GenerateMatchesResponse>;

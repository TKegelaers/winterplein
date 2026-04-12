using Winterplein.Shared.DTOs;

namespace Winterplein.Application.Commands.AddPlayer;

public record AddPlayerCommand(string FirstName, string LastName, GenderDto Gender);

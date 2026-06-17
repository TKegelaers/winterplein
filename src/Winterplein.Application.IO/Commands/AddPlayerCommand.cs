using Winterplein.Application.IO.DTOs;

namespace Winterplein.Application.IO.Commands;

public record AddPlayerCommand(string FirstName, string LastName, GenderDto Gender);

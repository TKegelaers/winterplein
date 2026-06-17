namespace Winterplein.Application.IO.DTOs;

public record AddPlayerRequest(string FirstName, string LastName, GenderDto Gender);

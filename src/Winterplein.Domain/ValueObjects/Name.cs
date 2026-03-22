namespace Winterplein.Domain.ValueObjects;

public record Name
{
    public string FirstName { get; }
    public string LastName { get; }

    public Name(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("FirstName cannot be empty or whitespace.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("LastName cannot be empty or whitespace.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
    }
}

using Winterplein.Domain.ValueObjects;

namespace Winterplein.UnitTests.Common.Builders;

public class NameBuilder
{
    private string _firstName = "John";
    private string _lastName = "Doe";

    public NameBuilder WithFirstName(string firstName) { _firstName = firstName; return this; }
    public NameBuilder WithLastName(string lastName) { _lastName = lastName; return this; }

    public Name Build() => new(_firstName, _lastName);
}

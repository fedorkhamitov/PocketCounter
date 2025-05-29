using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public class Title : ValueObject
{
    public string Value { get; }

    private Title(string value)
    {
        Value = value;
    }

    public static Result<Title, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(value);
        return new Title(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
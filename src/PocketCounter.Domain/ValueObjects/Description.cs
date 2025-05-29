using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public class Description : ValueObject
{
    public string Value { get; }

    private Description(string value)
    {
        Value = value;
    }

    public static Result<Description, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(value);
        return new Description(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
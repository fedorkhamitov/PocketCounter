using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public class SerialNumber : ValueObject
{
    public int Value { get; }

    private SerialNumber(int value)
    {
        Value = value;
    }

    public static Result<SerialNumber, Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid("value (SerialNumber)");
        return new SerialNumber(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

/// <summary>
/// SKU (Stock Keeping Unit) — это уникальный код, который описывает единицу ассортимента.
/// Привло для SKU - начинается с кириллической буквы и заканчивается цифрой
/// </summary>
public class Sku : ValueObject
{
    public string Value { get; }

    private Sku(string value)
    {
        Value = value;
    }

    public static Result<Sku, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Errors.General.ValueIsRequired(value);
        
        // Проверка: начинается с кириллической буквы и заканчивается цифрой
        // ^[\u0400-\u04FF] - первая буква кириллицы (Юникод)
        // .*\d$ - заканчивается цифрой
        var regex = new System.Text.RegularExpressions.Regex(@"^[\u0400-\u04FF].*\d$");
        if (!regex.IsMatch(value))
            return Errors.General.ValueIsInvalid("Sku value");
        
        return new Sku(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
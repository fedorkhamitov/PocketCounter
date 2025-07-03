using CSharpFunctionalExtensions;
using PhoneNumbers;
using System.Text;
using PocketCounter.Domain.Share;
using ErrorType = PhoneNumbers.ErrorType;
using PhoneNumberType = PhoneNumbers.PhoneNumberType;


namespace PocketCounter.Domain.ValueObjects;

/// <summary>
/// <param name="Number">Номер телефона в формате E.164 (международный стандарт, всегда начинается с + и кода страны). (+79501234567)</param>
/// <param name="CountryCode">Код страны, выделенный из номера (например, для России — 7, для США — 1). (7)</param>
/// <param name="NationalNumber">Национальная часть номера без кода страны (обычно 10 цифр для России). (9501234567)</param>
/// <param name="InternationalFormat">Номер в международном человекочитаемом формате, с пробелами и дефисами, как принято в международных справочниках. (+7 950 123-45-67)</param>
/// <param name="Type">Тип телефонного номера (например, мобильный, стационарный, факс и т.д.). Значение определяется библиотекой. (MOBILE)</param>
/// </summary>
public class PhoneNumber : ValueObject
{
    private static readonly PhoneNumberUtil _phoneUtil = PhoneNumberUtil.GetInstance();

    public string Number { get; }
    public string CountryCode { get; }
    public string NationalNumber { get; }
    public string InternationalFormat { get; }
    public PhoneNumberType Type { get; }

    private PhoneNumber(
        string number,
        string countryCode,
        string nationalNumber,
        string internationalFormat,
        PhoneNumberType type)
    {
        Number = number;
        CountryCode = countryCode;
        NationalNumber = nationalNumber;
        InternationalFormat = internationalFormat;
        Type = type;
    }

    public static Result<PhoneNumber, Error> Create(string phoneNumber, string regionCode = "RU")
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return new PhoneNumber("","","","", PhoneNumberType.MOBILE);

        try
        {
            var parsedNumber = _phoneUtil.Parse(phoneNumber, regionCode);

            if (!_phoneUtil.IsValidNumber(parsedNumber))
                return Errors.General.ValueIsInvalid("parsedNumber");

            var numberType = _phoneUtil.GetNumberType(parsedNumber);
            if (numberType != (PhoneNumbers.PhoneNumberType)PhoneNumberType.MOBILE &&
                numberType != (PhoneNumbers.PhoneNumberType)PhoneNumberType.FIXED_LINE_OR_MOBILE)
                return Errors.General.ValueIsInvalid("parsedNumber (modile format)");

            return Result.Success<PhoneNumber, Error>(new PhoneNumber(
                _phoneUtil.Format(parsedNumber, PhoneNumberFormat.E164),
                parsedNumber.CountryCode.ToString(),
                parsedNumber.NationalNumber.ToString(),
                _phoneUtil.Format(parsedNumber, PhoneNumberFormat.INTERNATIONAL),
                (PhoneNumberType)numberType
            ));
        }
        catch (NumberParseException ex)
        {
            return Result.Failure<PhoneNumber, Error>(Error.Failure("fail", GetParseError(ex.ErrorType)));
        }
    }

    private static string GetParseError(ErrorType errorType) => errorType switch
    {
        ErrorType.INVALID_COUNTRY_CODE => "Неверный код страны",
        ErrorType.NOT_A_NUMBER => "Некорректный формат номера",
        ErrorType.TOO_SHORT_NSN => "Слишком короткий номер",
        ErrorType.TOO_LONG => "Превышена максимальная длина номера",
        _ => "Ошибка валидации номера"
    };

    public string ToNationalFormat() =>
        FormatNumber(InternationalFormat, " ", " (", ") ", "-");

    public string ToMaskedFormat(char maskChar = '*', int visibleDigits = 4) =>
        new StringBuilder(InternationalFormat)
            .Replace(CountryCode, $"+{CountryCode}")
            .Replace(NationalNumber,
                NationalNumber[..^visibleDigits] +
                new string(maskChar, visibleDigits))
            .ToString();

    private static string FormatNumber(string number, params string[] separators)
    {
        var sb = new StringBuilder();
        foreach (var c in number)
            sb.Append(char.IsDigit(c) || c == '+' ? c : separators[0]);
        return sb.ToString();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
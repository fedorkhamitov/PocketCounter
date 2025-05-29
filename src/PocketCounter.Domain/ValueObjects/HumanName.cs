using CSharpFunctionalExtensions;
using PocketCounter.Domain.Extensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public record HumanName
{
    public string FirstName { get; } = default!;
    public string Patronymic { get; } = default!;
    public string FamilyName { get; } = default!;

    private HumanName()
    {
    }

    private HumanName(string firstName, string patronymic, string familyName)
    {
        FirstName = firstName;
        Patronymic = patronymic;
        FamilyName = familyName;
    }

    public static Result<HumanName, Error> Create(string firstName, string patronymic, string familyName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired("First Name");
        if (string.IsNullOrWhiteSpace(patronymic))
            return Errors.General.ValueIsRequired("Patronymic");
        if (string.IsNullOrWhiteSpace(familyName) || !familyName.IsValidHumanName())
            return Errors.General.ValueIsRequired("Family Name");

        if (!firstName.IsValidHumanName())
            return Errors.General.ValueIsInvalid("First Name");
        if (!patronymic.IsValidHumanName())
            return Errors.General.ValueIsInvalid("Patronymic");
        if (!familyName.IsValidHumanName())
            return Errors.General.ValueIsInvalid("Family Name");

        return new HumanName(firstName, patronymic, familyName);
    }

    public override string ToString()
    {
        return FirstName + " " + Patronymic + " " + FamilyName;
    }
}
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

    public static Result<HumanName, Error> Create(string firstName, string familyName, string patronymic = "")
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Errors.General.ValueIsRequired("First Name");
        if (string.IsNullOrWhiteSpace(familyName) || !familyName.IsValidHumanName())
            return Errors.General.ValueIsRequired("Family Name");

        if (!firstName.IsValidHumanName())
            return Errors.General.ValueIsInvalid("First Name");
        if (!familyName.IsValidHumanName())
            return Errors.General.ValueIsInvalid("Family Name");

        return new HumanName(firstName, patronymic, familyName);
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Patronymic))
            return FirstName + " " + FamilyName;
        return FirstName + " " + Patronymic + " " + FamilyName;
    }
}
using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Create;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(n =>
                HumanName.Create(n.FirstName, n.Patronymic, n.FamilyName));

        RuleFor(r => r.PhoneNumber)
            .MustBeValueObject<CreateCustomerRequest, string, PhoneNumber>(p =>
                PhoneNumber.Create(p));
    }
}
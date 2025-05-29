using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Update;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
    public UpdateCustomerRequestValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(n =>
                HumanName.Create(n.FirstName, n.Patronymic, n.FamilyName));

        RuleFor(r => r.PhoneNumber)
            .MustBeValueObject<UpdateCustomerRequest, string, PhoneNumber>(p =>
                PhoneNumber.Create(p));
    }
}
﻿using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Create;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator()
    {
        RuleFor(r => r.FullName)
            .MustBeValueObject(n =>
                HumanName.Create(n.FirstName, n.FamilyName, n.Patronymic));

        /*RuleFor(r => r.PhoneNumber)
            .MustBeValueObject<CreateCustomerRequest, string, PhoneNumber>(p =>
                PhoneNumber.Create(p));*/
        
        RuleFor(r => r.PhoneNumber)
            .MustBeValueObject<CreateCustomerRequest, string, PhoneNumber>(p => PhoneNumber.Create(p))
            .When(r => !string.IsNullOrWhiteSpace(r.PhoneNumber));
    }
}
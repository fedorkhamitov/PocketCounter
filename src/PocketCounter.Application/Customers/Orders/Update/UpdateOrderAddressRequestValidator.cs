using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Update;

public class UpdateOrderAddressRequestValidator : AbstractValidator<UpdateOrderAddressRequest>
{
    public UpdateOrderAddressRequestValidator()
    {
        RuleFor(r => r.Address).MustBeValueObject(a =>
            Address.Create(
                a.ZipCode,
                a.Country,
                a.State,
                a.City,
                a.StreetName,
                a.StreetNumber,
                a.Apartment,
                a.SpecialAddressString));
    }
}
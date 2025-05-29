using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Create;

public class CreateOrderRequestValidation : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidation()
    {
        RuleForEach(r => r.CartLineDtos).MustBeValueObject(cl =>
            CartLine.Create(cl.ProductId, cl.Quantity));
        
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
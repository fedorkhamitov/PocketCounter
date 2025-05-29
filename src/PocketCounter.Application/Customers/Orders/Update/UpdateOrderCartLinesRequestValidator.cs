using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Update;

public class UpdateOrderCartLinesRequestValidator : AbstractValidator<UpdateOrderCartLinesRequest>
{
    public UpdateOrderCartLinesRequestValidator()
    {
        RuleForEach(r => r.CartLinesDtoForAdd).MustBeValueObject(cl =>
            CartLine.Create(cl.ProductId, cl.Quantity));
        
        RuleForEach(r => r.CartLinesDtoForRemove).MustBeValueObject(cl =>
            CartLine.Create(cl.ProductId, cl.Quantity));
    }
}
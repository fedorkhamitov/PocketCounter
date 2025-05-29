using FluentValidation;
using PocketCounter.Application.Validation;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductQuantityRequestValidator : AbstractValidator<UpdateProductQuantityRequest>
{
    public UpdateProductQuantityRequestValidator()
    {
        RuleFor(x => x.QuantityForShipping)
            .GreaterThanOrEqualTo(0).WithError("QuantityForShipping");

        RuleFor(x => x.ReservedQuantity)
            .GreaterThanOrEqualTo(0).WithError("ReservedQuantity");

        RuleFor(x => x.ActualQuantity)
            .GreaterThanOrEqualTo(0).WithError("ActualQuantity");
    }
}
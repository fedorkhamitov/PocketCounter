using FluentValidation;
using PocketCounter.Application.Validation;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductPriceRequestValidator : AbstractValidator<UpdateProductPriceRequest>
{
    public UpdateProductPriceRequestValidator()
    {
        RuleFor(x => x.Price)
            .GreaterThan(0).WithError("Price");

        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithError("CostPrice")
            .LessThanOrEqualTo(x => x.Price)
            .WithMessage("Себестоимость не может превышать цену")
            .WithError("CostPrice");
    }
}
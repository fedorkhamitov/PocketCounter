using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories.Products.Create;

public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductRequestValidator()
    {
        RuleFor(request => request.Sku).MustBeValueObject(s => Sku.Create(s));
        
        RuleFor(request => request.Title).MustBeValueObject(Title.Create);
        
        RuleFor(request => request.Description).MustBeValueObject(d => Description.Create(d));
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithError("Price");

        RuleFor(x => x.CostPrice)
            .GreaterThan(0).WithError("CostPrice")
            .LessThanOrEqualTo(x => x.Price)
            .WithMessage("Себестоимость не может превышать цену")
            .WithError("CostPrice");

        RuleFor(x => x.Width)
            .GreaterThanOrEqualTo(0).WithError("Width");

        RuleFor(x => x.Height)
            .GreaterThanOrEqualTo(0).WithError("Height");

        RuleFor(x => x.Depth)
            .GreaterThanOrEqualTo(0).WithError("Depth");

        RuleFor(x => x.Weigth)
            .GreaterThanOrEqualTo(0).WithError("Weigth");

        RuleFor(x => x.QuantityForShipping)
            .GreaterThanOrEqualTo(0).WithError("QuantityForShipping");

        RuleFor(x => x.ReservedQuantity)
            .GreaterThanOrEqualTo(0).WithError("ReservedQuantity");

        RuleFor(x => x.ActualQuantity)
            .GreaterThanOrEqualTo(0).WithError("ActualQuantity");
    }
}
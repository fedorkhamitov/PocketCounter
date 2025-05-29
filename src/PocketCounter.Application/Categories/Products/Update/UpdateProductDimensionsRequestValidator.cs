using FluentValidation;
using PocketCounter.Application.Validation;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductDimensionsRequestValidator : AbstractValidator<UpdateProductDimensionsRequest>
{
    public UpdateProductDimensionsRequestValidator()
    {
        RuleFor(x => x.Width)
            .NotEmpty().GreaterThan(0).WithError("Width");

        RuleFor(x => x.Height)
            .NotEmpty().GreaterThan(0).WithError("Height");

        RuleFor(x => x.Depth)
            .NotEmpty().GreaterThan(0).WithError("Depth");

        RuleFor(x => x.Weigth)
            .GreaterThan(0).GreaterThan(0).WithError("Weigth");
    }
}
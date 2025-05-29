using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductMainInfoRequestValidator : AbstractValidator<UpdateProductMainInfoRequest>
{
    public UpdateProductMainInfoRequestValidator()
    {
        RuleFor(request => request.Sku).MustBeValueObject(Sku.Create);
        
        RuleFor(request => request.Title).MustBeValueObject(Title.Create);
        
        RuleFor(request => request.Description).MustBeValueObject(Description.Create);
    }
}
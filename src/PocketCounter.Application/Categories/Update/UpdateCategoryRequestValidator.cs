using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Update;

public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .MaximumLength(Constants.MAX_LOW_TEXT_LENGHT)
            .WithError("Category Name");
        
        RuleFor(c => c.Description).NotEmpty()
            .MaximumLength(Constants.MAX_HIGH_TEXT_LENGHT)
            .WithError("Category Description");
    }
}
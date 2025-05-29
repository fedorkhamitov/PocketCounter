using FluentValidation;
using PocketCounter.Application.Validation;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Create;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .MaximumLength(Constants.MAX_LOW_TEXT_LENGHT)
            .WithError("Category Name");
    }
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Create;

public class CreateCategoryHandler(ICategoryRepository categoryRepository, ILogger<CreateCategoryHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = new Category(request.Name, request.Description);
       
        var result = await categoryRepository.Add(category, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        logger.LogInformation("Created new Category Id: {0}, Name: {1}", category.Id, category.Name);

        return category.Id;
    }
}
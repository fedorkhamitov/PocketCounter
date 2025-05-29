using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Update;

public class UpdateCategoryHandler(ICategoryRepository categoryRepository, ILogger<UpdateCategoryHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid id,
        UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(id, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;

        var categoryUpdated = categoryResult.Value.UpdateMainInfo(request.Name, request.Description);
        if (categoryUpdated.IsFailure)
            return categoryUpdated.Error;
        
        var result = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Updated Category with Id: {0}, Name: {1}", 
            categoryResult.Value.Id, categoryResult.Value.Name);

        return result;
    }
}
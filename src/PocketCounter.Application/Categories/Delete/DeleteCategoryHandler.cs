using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Delete;

public class DeleteCategoryHandler(ICategoryRepository categoryRepository, ILogger<DeleteCategoryHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(Guid id, bool isHardDelete, CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(id, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;
        
        if (isHardDelete)
        {
            var resultHardDelete = await categoryRepository.Remove(categoryResult.Value, cancellationToken);
            
            logger.LogInformation("Removed Category with Id: {0}, Title: {1}", 
                categoryResult.Value.Id, categoryResult.Value.Name);
            
            return resultHardDelete;
        }
        
        categoryResult.Value.Delete();

        var resultSoftDelete = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Deleted (soft) Category with Id: {0}, Name: {1}", 
            categoryResult.Value.Id, categoryResult.Value.Name);

        return resultSoftDelete;
    }
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Products.Delete;

public class DeleteProductHandler(ICategoryRepository categoryRepository, ILogger<DeleteProductHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid id, 
        Guid productId, 
        bool isHardDelete, 
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(id, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;

        var productResult = categoryResult.Value.Products.Single(p => p.Id == productId);
        
        if (isHardDelete)
        {
            categoryResult.Value.RemoveProduct(productResult);
            
            var resultHardDelete = await categoryRepository.Save(categoryResult.Value, cancellationToken);
            
            logger.LogInformation("Removed Product with Id: {0}, Title: {1}", 
                productResult.Id, productResult.Title);
            
            return resultHardDelete;
        }
        
        productResult.Delete();

        var resultSoftDelete = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Deleted (soft) Product with Id: {0}, Title: {1} from Category Id: {2}, Name: {3}", 
            productResult.Id, productResult.Title.Value, resultSoftDelete, categoryResult.Value.Name);

        return productResult.Id;
    }
}
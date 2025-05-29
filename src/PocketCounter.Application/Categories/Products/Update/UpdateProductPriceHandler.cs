using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductPriceHandler(
    ICategoryRepository categoryRepository, 
    ILogger<UpdateProductMainInfoHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid categoryId,
        Guid productId,
        UpdateProductPriceRequest request,
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(categoryId, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;
        
        var productResult = categoryResult.Value.Products.Single(p => p.Id == productId);

        var updateProductResult = productResult.UpdatePrices(request.Price, request.CostPrice);
        if (updateProductResult.IsFailure)
            return updateProductResult.Error;
        
        var updateCategoryResult = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Updated prices for Product (id: {0}) from category id: {1}", 
            productResult.Id, updateCategoryResult);

        return productResult.Id;
    }
}
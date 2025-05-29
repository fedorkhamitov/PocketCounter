using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductQuantityHandler(
    ICategoryRepository categoryRepository, 
    ILogger<UpdateProductMainInfoHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid categoryId,
        Guid productId,
        UpdateProductQuantityRequest request,
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(categoryId, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;
        
        var productResult = categoryResult.Value.Products.Single(p => p.Id == productId);

        var productQuantity = ProductQuantity.Create(
            request.QuantityForShipping,
            request.ReservedQuantity,
            request.ActualQuantity);
        if (productQuantity.IsFailure)
            return productQuantity.Error;

        var updateProductResult = productResult.UpdateProductQuantity(productQuantity.Value);
        if (updateProductResult.IsFailure)
            return updateProductResult.Error;
        
        var updateCategoryResult = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Updated Product Quantity for Product (id: {0}) from category id: {1}", 
            productResult.Id, updateCategoryResult);

        return productResult.Id;
    }
}
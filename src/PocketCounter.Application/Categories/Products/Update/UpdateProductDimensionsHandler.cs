using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductDimensionsHandler(
    ICategoryRepository categoryRepository, 
    ILogger<UpdateProductMainInfoHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid categoryId,
        Guid productId,
        UpdateProductDimensionsRequest request,
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(categoryId, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;
        
        var productResult = categoryResult.Value.Products.Single(p => p.Id == productId);

        var dimensionsResult = Dimensions.Create(request.Width, request.Height, request.Depth);
        if (dimensionsResult.IsFailure)
            return dimensionsResult.Error;

        var updateProductResult = productResult.UpdateDimensions(dimensionsResult.Value, request.Weigth);
        if (updateProductResult.IsFailure)
            return updateProductResult.Error;
        
        var updateCategoryResult = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Updated Product Quantity for Product (id: {0}) from category id: {1}", 
            productResult.Id, updateCategoryResult);

        return productResult.Id;
    }
}
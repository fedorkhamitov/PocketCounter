using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories.Products.Update;

public class UpdateProductMainInfoHandler(
    ICategoryRepository categoryRepository, 
    ILogger<UpdateProductMainInfoHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid categoryId,
        Guid productId,
        UpdateProductMainInfoRequest request,
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(categoryId, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;
        
        var productResult = categoryResult.Value.Products.Single(p => p.Id == productId);

        var sku = Sku.Create(request.Sku);
        if (sku.IsFailure)
            return sku.Error;

        var title = Title.Create(request.Title);
        if (title.IsFailure)
            return title.Error;
        
        var description = Description.Create(request.Description);
        if (description.IsFailure)
            return description.Error;

        var updateProductResult = productResult.UpdateMainInfo(sku.Value, title.Value, description.Value);
        if (updateProductResult.IsFailure)
            return updateProductResult.Error;
        
        var updateCategoryResult = await categoryRepository.Save(categoryResult.Value, cancellationToken);
        
        logger.LogInformation("Updated main info for Product (id: {0}) from category id: {1}", 
            productResult.Id, updateCategoryResult);

        return productResult.Id;
    }
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Categories.Products.Create;

public class CreateProductHandler(ICategoryRepository categoryRepository, ILogger<CreateProductHandler> logger)
{
    private readonly ILogger<CreateProductHandler> _logger = logger;

    public async Task<Result<Guid, Error>> Handle(
        CreateProductRequest request,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var categoryResult = await categoryRepository.GetById(categoryId, cancellationToken);
        if (categoryResult.IsFailure)
            return categoryResult.Error;

        var sku = Sku.Create(request.Sku).Value;

        var title = Title.Create(request.Title).Value;

        var description = Description.Create(request.Description).Value;

        var dimensions = Dimensions.Create(request.Width, request.Height, request.Depth).Value;

        var productQuantity = ProductQuantity.Create(
            request.QuantityForShipping, 
            request.ReservedQuantity,
            request.ActualQuantity).Value;

        var product = new Product(
            sku, 
            title, 
            description, 
            request.Price, 
            request.CostPrice, 
            dimensions, 
            request.Weigth,
            productQuantity);

        var isProductExistsByTitle = await categoryRepository
            .IsProductAlreadyExistsByTitle(title, cancellationToken);
        if (isProductExistsByTitle.IsFailure)
            return isProductExistsByTitle.Error;

        if (isProductExistsByTitle.Value == true)
            return Errors.General.ValueIsAlreadyExists("Product (by title)");

        var isProductExistsBySku = await categoryRepository
            .IsProductAlreadyExistsBySku(sku, cancellationToken);
        
        if (isProductExistsBySku.IsFailure)
            return isProductExistsBySku.Error;

        if (isProductExistsBySku.Value == true)
            return Errors.General.ValueIsAlreadyExists("Product (by SKU)");
        
        categoryResult.Value.AddProduct(product);
        
        await categoryRepository.Save(categoryResult.Value, cancellationToken);

        return product.Id;
    }
}
using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Domain.Entities;

public class Product : SoftDeletableEntity
{
    private Product() { }

    public Product(
        Sku sku, 
        Title title, 
        Description description, 
        decimal price, 
        decimal costPrice, 
        Dimensions dimensions, 
        double weigth, 
        ProductQuantity productQuantity)
    {
        Id = new Guid();
        Sku = sku;
        Title = title;
        Description = description;
        Price = price;
        CostPrice = costPrice;
        Dimensions = dimensions;
        Weigth = weigth;
        ProductQuantity = productQuantity;
    }

    public new Guid Id { get; private set; }
    
    public Sku Sku { get; private set; } = default!;
    
    public Title Title {  get; private set; } = default!;
    
    public Description Description { get; private set; } = default!;
    
    public decimal Price { get; private set; } = 0;
    
    public decimal CostPrice { get; private set; } = 0;

    public PhotoFilesList PhotosList { get; private set; } = new();
    
    public Dimensions Dimensions { get; private set; } = default!;
    
    public double Weigth { get; private set; }
    
    public ProductQuantity ProductQuantity { get; private set; } = default!;

    // --- Methods ---
    
    public UnitResult<Error> UpdateMainInfo(Sku sku, Title title, Description description)
    {
        Sku = sku;
        Title = title;
        Description = description;
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdatePrices(decimal price, decimal costPrice)
    {
        if (price <= 0) 
            return Errors.General.ValueIsRequired("Price");
        
        if (costPrice <= 0) 
            return Errors.General.ValueIsRequired("CostPrice");

        if (costPrice > price)
            return Errors.General.ValueIsInvalid("CostPrice");

        Price = price;
        CostPrice = costPrice;
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdateDimensions(Dimensions dimensions, double weigth)
    {
        if (dimensions.Width >= 0 || dimensions.Height >= 0 || dimensions.Depth >= 0)
            return Errors.General.ValueIsInvalid("dimensions");
        
        if (weigth >= 0)
            return Errors.General.ValueIsInvalid("weigth");

        Dimensions = dimensions;
        Weigth = weigth;
        
        return Result.Success<Error>();
    }

    public UnitResult<Error> UpdateProductQuantity(ProductQuantity productQuantity)
    {
        if (productQuantity.ActualQuantity < 0 ||
            productQuantity.ReservedQuantity < 0 ||
            productQuantity.QuantityForShipping < 0)
            return Errors.General.ValueIsInvalid("productQuantity");

        ProductQuantity = productQuantity;
        
        return Result.Success<Error>();
    }

    public Result<decimal, Error> GetTotalPriceForOrderQuantity(int quantity)
    {
        if (quantity < 0)
            return Errors.General.ValueIsInvalid("quantity");
        return quantity * Price;
    }
}
namespace PocketCounter.Application.Categories.Products.Create;

public record CreateProductRequest(
    string Sku,
    string Title,
    string Description,
    decimal Price,
    decimal CostPrice,
    double Width,
    double Height,
    double Depth,
    double Weigth,
    int QuantityForShipping,
    int ReservedQuantity,
    int ActualQuantity);
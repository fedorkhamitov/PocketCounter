namespace PocketCounter.Application.Dtos;

public class ProductDto
{
    public Guid Id { get; init; }
    public string Sku { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public decimal Price { get; init; } = 0;
    public decimal CostPrice { get; init; } = 0;
    public DimensionsDto Dimensions { get; init; } = default!;
    public double Weigth { get; init; }
    public int QuantityForShipping { get; init; }
    public int ReservedQuantity { get; init; }
    public int ActualQuantity { get; init; }
    public Guid CategoryId { get; init; }
}
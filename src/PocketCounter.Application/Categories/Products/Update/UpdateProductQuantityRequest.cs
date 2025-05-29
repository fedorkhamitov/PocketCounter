namespace PocketCounter.Application.Categories.Products.Update;

public record UpdateProductQuantityRequest(int ActualQuantity, int ReservedQuantity, int QuantityForShipping);
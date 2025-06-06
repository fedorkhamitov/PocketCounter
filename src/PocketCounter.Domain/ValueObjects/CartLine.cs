using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public class CartLine : ValueObject
{
    public Guid ProductId { get; }
    public int Quantity { get; }

    [JsonConstructor]
    private CartLine(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }

    public static Result<CartLine, Error> Create(Guid productId, int quantity)
    {
        if (quantity <= 0)
            return Errors.General.ValueIsRequired("Quantity");
        return new CartLine(productId, quantity);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Quantity;
    }
}
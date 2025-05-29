using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Domain.Entities;

public class Order : SoftDeletableEntity
{
    public Order()
    {
    }

    public Order(
        List<CartLine> cartLines,
        Address address,
        decimal totalPrice,
        string comment)
    {
        Id = new Guid();
        _cartLines = cartLines;
        Address = address;
        Status = OrderStatus.CreatedOnly;
        IsPaid = false;
        CreateDateTime = DateTime.UtcNow;
        Comment = comment;
        TotalPrice = totalPrice;
    }

    public new Guid Id { get; private set; }

    public IReadOnlyList<CartLine> CartLines => _cartLines;

    private readonly List<CartLine> _cartLines = [];

    public Address Address { get; private set; } = default!;

    public DateTime CreateDateTime { get; private set; }

    public string Comment { get; private set; } = string.Empty;

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public bool IsPaid { get; private set; } // Оплачен/нет

    // --- Methods ---

    public UnitResult<Error> AddCartLine(CartLine cartLine)
    {
        if (cartLine.Quantity < 0)
            return Errors.General.ValueIsInvalid(nameof(cartLine.Quantity));

        var existingLine = _cartLines
            .FirstOrDefault(cl => cl.ProductId == cartLine.ProductId);

        if (existingLine != null)
        {
            var newQuantity = existingLine.Quantity + cartLine.Quantity;
            var newLineResult = CartLine.Create(existingLine.ProductId, newQuantity);
        
            if (newLineResult.IsFailure)
                return newLineResult.Error;

            var index = _cartLines.IndexOf(existingLine);
            _cartLines[index] = newLineResult.Value;
        }
        else
        {
            _cartLines.Add(cartLine);
        }

        return Result.Success<Error>();
    }

    public UnitResult<Error> RemoveCartLine(CartLine cartLine)
    {
        _cartLines.Remove(cartLine);
        
        return Result.Success<Error>();
    }

    public void UpdateStatus(OrderStatus orderStatus, bool isPaid)
    {
        Status = orderStatus;
        IsPaid = isPaid;
    }
}
using System.Runtime.InteropServices.JavaScript;
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
        //SerialNumber serialNumber,
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
        //SerialNumber = serialNumber;
    }

    public new Guid Id { get; private set; }

    public IReadOnlyList<CartLine> CartLines => _cartLines;

    private readonly List<CartLine> _cartLines = [];
    
    //public SerialNumber SerialNumber { get; private set; }
    
    public int OrderNumber { get; private set; }

    public Address Address { get; private set; } = default!;

    public DateTime CreateDateTime { get; private set; }

    public string Comment { get; private set; } = string.Empty;

    public OrderStatus Status { get; private set; }

    public decimal TotalPrice { get; private set; }

    public bool IsPaid { get; private set; } // Оплачен/нет

    // --- Methods ---

    public UnitResult<Error> SetTotalPrice(decimal newTotalPrice)
    {
        if (newTotalPrice <= 0)
            return Errors.General.ValueIsInvalid(nameof(newTotalPrice));
        
        TotalPrice = newTotalPrice;
        return Result.Success<Error>();
    }
    //public void SetSerialNumber(SerialNumber serialNumber) => SerialNumber = serialNumber;

    public UnitResult<Error> AddCartLine(CartLine cartLine)
    {
        if (cartLine.Quantity < 0)
            return Errors.General.ValueIsInvalid(nameof(cartLine.Quantity));

        var existingLine = _cartLines
            .FirstOrDefault(cl => cl.ProductId == cartLine.ProductId);
        if (existingLine is not null && cartLine.Quantity > existingLine.Quantity)
            return Errors.General.ValueIsInvalid(nameof(cartLine.Quantity));

        if (existingLine is not null)
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
        if (cartLine.Quantity < 0)
            return Errors.General.ValueIsInvalid(nameof(cartLine.Quantity));

        var existingLine = _cartLines
            .FirstOrDefault(cl => cl.ProductId == cartLine.ProductId);
        
        if (existingLine is not null && cartLine.Quantity > existingLine.Quantity)
            return Errors.General.ValueIsInvalid(nameof(cartLine.Quantity));
        
        if (existingLine is not null && existingLine.Quantity == cartLine.Quantity)
        {
            _cartLines.Remove(cartLine);
            return Result.Success<Error>();
        }

        if (existingLine is null) return Result.Success<Error>();
        
        var newQuantity = existingLine.Quantity - cartLine.Quantity;
        var newLineResult = CartLine.Create(existingLine.ProductId, newQuantity);
        
        if (newLineResult.IsFailure)
            return newLineResult.Error;

        var index = _cartLines.IndexOf(existingLine);
        _cartLines[index] = newLineResult.Value;

        return Result.Success<Error>();
    }

    public void UpdateStatus(OrderStatus orderStatus, bool isPaid)
    {
        Status = orderStatus;
        IsPaid = isPaid;
    }

    public void UpdateAddress(Address address)
    {
        Address = address;
    }
    
    public void SetOrderNumber(int number)
    {
        if (number <= 0) 
            throw new ArgumentException("Order number must be positive");
        OrderNumber = number;
    }
}
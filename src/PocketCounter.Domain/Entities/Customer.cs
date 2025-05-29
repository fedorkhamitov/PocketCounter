using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Domain.Entities;

public class Customer : SoftDeletableEntity
{
    private Customer() { }

    public Customer(HumanName name, PhoneNumber phoneNumber)
    {
        Id = Guid.NewGuid();
        Name = name;
        PhoneNumber = phoneNumber;
    }

    public new Guid Id { get; private set; }
    public HumanName Name { get; private set; } = default!;
    public PhoneNumber PhoneNumber { get; private set; } = default!;

    public IReadOnlyList<Order> Orders => _orders;
    
    private readonly List<Order> _orders = [];

    // --- Methods ---
    public UnitResult<Error> Update(HumanName name, PhoneNumber phoneNumber)
    {
        Name = name;
        PhoneNumber = phoneNumber;

        return Result.Success<Error>();
    }
    
    public UnitResult<Error> AddOrder(Order order)
    {
        _orders.Add(order);
        return Result.Success<Error>();
    }
    
    public UnitResult<Error> RemoveOrder(Order order)
    {
        _orders.Remove(order);
        return Result.Success<Error>();
    }
}
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Dtos;

public class OrderDto
{
    public Guid Id { get; init; }
    public AddressDto Address { get; init; } = default!;
    public DateTime CreateDateTime { get; init; }
    public string Comment { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public decimal TotalPrice { get; init; }
    public bool IsPaid { get; init; }
    public CartLineDto[] CartLines = [];
    public Guid CustomerId { get; init; }
}
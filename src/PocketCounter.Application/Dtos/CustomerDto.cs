namespace PocketCounter.Application.Dtos;

public class CustomerDto
{
    public Guid Id { get; init; }
    public HumanNameDto Name { get; init; } = default!;
    public string PhoneNumber { get; init; } = string.Empty;
    public OrderDto[] Orders = [];
}
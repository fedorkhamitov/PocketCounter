using PocketCounter.Application.Dtos;

namespace PocketCounter.Application.Customers.Orders.Create;

public record CreateOrderRequest(
    List<CartLineDto> CartLineDtos,
    AddressDto Address, 
    string Comment);
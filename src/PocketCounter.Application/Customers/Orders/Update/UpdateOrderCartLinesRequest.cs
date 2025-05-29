using PocketCounter.Application.Dtos;

namespace PocketCounter.Application.Customers.Orders.Update;

public record UpdateOrderCartLinesRequest(
    IEnumerable<CartLineDto>? CartLinesDtoForAdd,
    IEnumerable<CartLineDto>? CartLinesDtoForRemove);
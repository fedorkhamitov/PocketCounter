using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Customers.Orders.Update;

public record UpdateOrderStatusRequest(OrderStatus Status, bool IsPaid);
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Application.Database;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Update;

public class UpdateOrderCartLinesHandler(
    ICustomerRepository customerRepository,
    IReadDbContext readDbContext,
    ILogger<UpdateOrderCartLinesHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid customerId,
        Guid orderId,
        UpdateOrderCartLinesRequest cartLinesRequest,
        CancellationToken cancellationToken)
    {
        var orderResult = await customerRepository.GetOrderById(orderId, cancellationToken);
        if (orderResult.IsFailure)
            return orderResult.Error;

        var customer = await customerRepository.GetById(customerId, cancellationToken);

        if (cartLinesRequest.CartLinesDtoForAdd is null && cartLinesRequest.CartLinesDtoForRemove is null)
            return Error.Conflict(
                "null.components",
                "CartLinesDtoForAdd & CartLinesDtoForRemove are NULL");

        if (cartLinesRequest.CartLinesDtoForAdd is not null)
        {
            var cartLines = cartLinesRequest.CartLinesDtoForAdd.Select(cl =>
                (CartLine.Create(cl.ProductId, cl.Quantity)).Value).ToList();

            foreach (var cartLine in cartLines)
            {
                orderResult.Value.AddCartLine(cartLine);
            }
        }

        if (cartLinesRequest.CartLinesDtoForRemove is not null)
        {
            var cartLines = cartLinesRequest.CartLinesDtoForRemove.Select(cl =>
                (CartLine.Create(cl.ProductId, cl.Quantity)).Value).ToList();

            foreach (var cartLine in cartLines)
            {
                orderResult.Value.RemoveCartLine(cartLine);
            }
        }

        orderResult.Value.SetTotalPrice(NewTotalPrice(orderResult.Value.CartLines.ToArray()));

        var customerIdResult = await customerRepository.Save(customer, cancellationToken);
        
        logger.LogInformation("Updated cart lines for order id: {0} from customer id: {1}", 
            orderResult.Value.Id, customerIdResult);
        
        return orderResult.Value.Id;
    }

    private decimal NewTotalPrice(CartLine[] cartLines)
    {
        var products = readDbContext.Products
            .Where(p => cartLines.Select(c => c.ProductId).ToArray().Contains(p.Id))
            .ToList();

        var result = (from product in products 
            let quantity = cartLines.FirstOrDefault(c => c.ProductId == product.Id)!.Quantity 
            select product.Price * quantity).Sum();
        
        return result;
    }
}
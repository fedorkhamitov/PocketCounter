using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Application.Categories;
using PocketCounter.Application.Database;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Update;

public class UpdateOrderStatusHandler(
    ICustomerRepository customerRepository,
    ICategoryRepository categoryRepository,
    IReadDbContext readDbContext,
    ILogger<UpdateOrderStatusHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid customerId,
        Guid orderId,
        UpdateOrderStatusRequest statusRequest,
        CancellationToken cancellationToken)
    {
        var orderResult = await customerRepository.GetOrderById(orderId, cancellationToken);
        if (orderResult.IsFailure)
            return orderResult.Error;

        var oldStatus = orderResult.Value.Status;
        
        if (oldStatus == statusRequest.Status)
            return orderId;
        
        orderResult.Value.UpdateStatus(statusRequest.Status, statusRequest.IsPaid);
        
        var customer = await customerRepository.GetById(customerId, cancellationToken);
        
        var customerIdResult = await customerRepository.Save(customer, cancellationToken);
        
        foreach (var cartLine in orderResult.Value.CartLines)
        {
            var product = await categoryRepository
                .GetProductById(cartLine.ProductId, cancellationToken);
            if (product.IsFailure)
                return product.Error;

            if (product.Value.ProductQuantity.ActualQuantity < cartLine.Quantity)
                return Error.Conflict("quantity.conflict", "New actual quantity lower then old.");

            var newProductQuantity = CorrectedQuantity(
                oldStatus, 
                statusRequest.Status, 
                product.Value.ProductQuantity, 
                cartLine.Quantity);
            if (newProductQuantity.IsFailure)
                return newProductQuantity.Error;
            
            product.Value.UpdateProductQuantity(newProductQuantity.Value);

            var productDto = readDbContext.Products.FirstOrDefault(p => p.Id == product.Value.Id);
            if (productDto is null)
                return Errors.General.NotFound(product.Value.Id);

            var category = await categoryRepository
                .GetById(productDto.CategoryId, cancellationToken);
            if (category.IsFailure)
                return category.Error;

            await categoryRepository.Save(category.Value, cancellationToken);
        }

        logger.LogInformation("Updated status for order id: {0} from customer id: {1}",
            orderResult.Value.Id, customerIdResult);

        return orderResult.Value.Id;
    }

    private static Result<ProductQuantity, Error> CorrectedQuantity(
        OrderStatus oldStatus,
        OrderStatus newStatus,
        ProductQuantity oldQuantity,
        int cartLineQuantity)
    {
        switch (oldStatus)
        {
            case OrderStatus.CreatedOnly:
                switch (newStatus)
                {
                    case OrderStatus.Completed or OrderStatus.Deferred:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping + cartLineQuantity,
                            oldQuantity.ReservedQuantity,
                            oldQuantity.ActualQuantity);
                    case OrderStatus.Reserved or OrderStatus.PartlyReserved:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping,
                            oldQuantity.ReservedQuantity + cartLineQuantity,
                            oldQuantity.ActualQuantity);
                    case OrderStatus.Shipped:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping,
                            oldQuantity.ReservedQuantity,
                            oldQuantity.ActualQuantity - cartLineQuantity);
                    //----
                    case OrderStatus.None:
                    case OrderStatus.CreatedOnly:
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
                }

            case OrderStatus.Completed or OrderStatus.Deferred:
                switch (newStatus)
                {
                    case OrderStatus.CreatedOnly:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping - cartLineQuantity,
                            oldQuantity.ReservedQuantity,
                            oldQuantity.ActualQuantity);
                    case OrderStatus.Reserved or OrderStatus.PartlyReserved:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping - cartLineQuantity,
                            oldQuantity.ReservedQuantity + cartLineQuantity,
                            oldQuantity.ActualQuantity);
                    case OrderStatus.Shipped:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping - cartLineQuantity,
                            oldQuantity.ReservedQuantity,
                            oldQuantity.ActualQuantity - cartLineQuantity);
                    //----
                    case OrderStatus.Completed or OrderStatus.Deferred:
                    case OrderStatus.None:
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
                }

            case OrderStatus.Reserved or OrderStatus.PartlyReserved:
                switch (newStatus)
                {
                    case OrderStatus.CreatedOnly:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping,
                            oldQuantity.ReservedQuantity - cartLineQuantity,
                            oldQuantity.ActualQuantity);
                    case OrderStatus.Completed or OrderStatus.Deferred:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping + cartLineQuantity,
                            oldQuantity.ReservedQuantity - cartLineQuantity,
                            oldQuantity.ActualQuantity);
                    case OrderStatus.Shipped:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping,
                            oldQuantity.ReservedQuantity - cartLineQuantity,
                            oldQuantity.ActualQuantity - cartLineQuantity);
                    //----
                    case OrderStatus.Reserved or OrderStatus.PartlyReserved:
                    case OrderStatus.None:
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
                }

            case OrderStatus.Shipped:
                switch (newStatus)
                {
                    case OrderStatus.CreatedOnly:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping,
                            oldQuantity.ReservedQuantity,
                            oldQuantity.ActualQuantity + cartLineQuantity);
                    case OrderStatus.Completed or OrderStatus.Deferred:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping + cartLineQuantity,
                            oldQuantity.ReservedQuantity,
                            oldQuantity.ActualQuantity + cartLineQuantity);
                    case OrderStatus.Reserved or OrderStatus.PartlyReserved:
                        return ProductQuantity.Create(
                            oldQuantity.QuantityForShipping,
                            oldQuantity.ReservedQuantity + cartLineQuantity,
                            oldQuantity.ActualQuantity + cartLineQuantity);
                    //----
                    case OrderStatus.Shipped:
                    case OrderStatus.None:
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, null);
                }
            case OrderStatus.None:
            default:
                throw new ArgumentOutOfRangeException(nameof(oldStatus), oldStatus, null);
        }
    }
}
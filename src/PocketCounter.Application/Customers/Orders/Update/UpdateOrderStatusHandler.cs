using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Customers.Orders.Update;

public class UpdateOrderStatusHandler(
    ICustomerRepository customerRepository,
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

        var customer = await customerRepository.GetById(customerId, cancellationToken);
        
        orderResult.Value.UpdateStatus(statusRequest.Status, statusRequest.IsPaid);
        
        var customerIdResult = await customerRepository.Save(customer, cancellationToken);
        
        logger.LogInformation("Updated status for order id: {0} from customer id: {1}", 
            orderResult.Value.Id, customerIdResult);
        
        return orderResult.Value.Id;
    }
}
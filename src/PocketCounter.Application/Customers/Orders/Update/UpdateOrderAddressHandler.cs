using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Update;

public class UpdateOrderAddressHandler(
    ICustomerRepository customerRepository,
    ILogger<UpdateOrderAddressHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid customerId,
        Guid orderId, 
        UpdateOrderAddressRequest request, 
        CancellationToken cancellationToken)
    {
        var orderResult = await customerRepository.GetOrderById(orderId, cancellationToken);
        if (orderResult.IsFailure)
            return orderResult.Error;

        var newAddress = Address.Create(
            request.Address.ZipCode,
            request.Address.Country,
            request.Address.State,
            request.Address.City,
            request.Address.StreetName,
            request.Address.StreetNumber,
            request.Address.Apartment,
            request.Address.SpecialAddressString);
        if (newAddress.IsFailure)
            return newAddress.Error;
        
        orderResult.Value.UpdateAddress(newAddress.Value);
        
        var customer = await customerRepository.GetById(customerId, cancellationToken);

        var customerIdResult = await customerRepository.Save(customer, cancellationToken);
        
        logger.LogInformation("Updated address for order id: {0} from customer id: {1}", 
            orderResult.Value.Id, customerIdResult);
        
        return orderResult.Value.Id;
    }
}
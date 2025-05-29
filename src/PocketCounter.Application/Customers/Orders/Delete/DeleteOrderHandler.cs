using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Customers.Orders.Delete;

public class DeleteOrderHandler(ICustomerRepository customerRepository, ILogger<DeleteOrderHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid id, 
        Guid orderId, 
        bool isHardDelete, 
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetById(id, cancellationToken);

        var order = customer.Orders.Single(p => p.Id == orderId);
        
        if (isHardDelete)
        {
            customer.RemoveOrder(order);
            
            var resultHardDelete = await customerRepository.Save(customer, cancellationToken);
            
            logger.LogInformation("Removed Order with Id: {0}", order.Id);
            
            return resultHardDelete;
        }
        
        order.Delete();

        var resultSoftDelete = await customerRepository.Save(customer, cancellationToken);
        
        logger.LogInformation("Deleted (soft) Order with Id: {0} from customer id: {1}", 
            order.Id, resultSoftDelete);

        return order.Id;
    }
}
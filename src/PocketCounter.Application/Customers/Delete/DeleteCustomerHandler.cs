using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Customers.Delete;

public class DeleteCustomerHandler(ICustomerRepository customerRepository, ILogger<DeleteCustomerHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(Guid id, bool isHardDelete, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetById(id, cancellationToken);
        
        if (isHardDelete)
        {
            var resultHardDelete = await customerRepository.Remove(customer, cancellationToken);
            
            logger.LogInformation("Removed Customer with Id: {0}, Name: {1}", 
                customer.Id, customer.Name);
            
            return resultHardDelete;
        }
        
        customer.Delete();

        var resultSoftDelete = await customerRepository.Save(customer, cancellationToken);
        
        logger.LogInformation("Deleted (soft) Customer with Id: {0}, Name: {1}", 
            customer.Id, customer.Name);

        return resultSoftDelete;
    }
}
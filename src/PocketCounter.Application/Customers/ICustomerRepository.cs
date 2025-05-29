using CSharpFunctionalExtensions;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;

namespace PocketCounter.Application.Customers;

public interface ICustomerRepository
{
    Task<Result<Guid, Error>> Add(Customer customer, CancellationToken cancellationToken);
    
    Task<Customer> GetById(Guid id, CancellationToken cancellationToken);

    Task<Guid> Save(Customer customer, CancellationToken cancellationToken);
    
    Task<Result<Order, Error>> GetOrderById(Guid id, CancellationToken cancellationToken);
    
    Task<Guid> Remove(Customer category, CancellationToken cancellationToken);
}
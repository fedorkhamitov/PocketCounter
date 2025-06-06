using CSharpFunctionalExtensions;
using PocketCounter.Application.Database;
using PocketCounter.Application.Dtos;
using PocketCounter.Domain.Share;
using Microsoft.EntityFrameworkCore;

namespace PocketCounter.Application.Customers.Queries.GetCustomersList;

public class GetCustomersListHandler(IReadDbContext readDbContext)
{
    private readonly IReadDbContext _readDbContext = readDbContext;
    
    public async Task<Result<List<CustomerDto>, Error>> Handle(CancellationToken cancellationToken)
    {
        return await _readDbContext.Customers
            .ToListAsync(cancellationToken);
    }
}
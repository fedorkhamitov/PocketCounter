using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PocketCounter.Application.Customers;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Infrastructure.DbContexts.Write;

namespace PocketCounter.Infrastructure.Repositories;

public class CustomerRepository(WriteDbContext context) : ICustomerRepository
{
    public async Task<Result<Guid, Error>> Add(Customer customer, CancellationToken cancellationToken)
    {
        var isPhoneNumberAlreadyExists = await context.Customers
            .AnyAsync(c => 
                EF.Functions.ILike(c.PhoneNumber.Number, 
                    customer.PhoneNumber.Number), cancellationToken);
        if (isPhoneNumberAlreadyExists)
            return Errors.General.ValueIsAlreadyExists("Customer with same phone number");

        await context.Customers.AddAsync(customer, cancellationToken);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return customer.Id;
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
        {
            return Errors.General.ValueIsAlreadyExists("Category Name");
        }
    }

    public async Task<Customer> GetById(Guid id, CancellationToken cancellationToken)
    {
        var customer = await context.Customers
            .Include(c => c.Orders)
            .SingleAsync(c => c.Id == id, cancellationToken);

        return customer;
    }

    public async Task<Guid> Save(Customer customer, CancellationToken cancellationToken)
    {
        context.Customers.Attach(customer);

        await context.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }

    public async Task<Guid> Remove(Customer customer, CancellationToken cancellationToken)
    {
        context.Customers.Remove(customer);

        await context.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
    
    public async Task<Result<Order, Error>> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var customers = await context.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Orders.Any(p => p.Id == id), cancellationToken);

        return customers == null ? 
            Errors.General.NotFound(id)
            : customers.Orders.Single(p => p.Id == id);
    }
}
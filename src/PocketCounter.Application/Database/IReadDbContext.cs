using Microsoft.EntityFrameworkCore;
using PocketCounter.Application.Dtos;

namespace PocketCounter.Application.Database;

public interface IReadDbContext
{
    DbSet<CategoryDto> Categories { get; }
    DbSet<ProductDto> Products { get; }
    DbSet<CustomerDto> Customers { get; }
    DbSet<OrderDto> Orders { get; }
}
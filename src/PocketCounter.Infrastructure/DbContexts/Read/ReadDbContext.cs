using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocketCounter.Application.Database;
using PocketCounter.Application.Dtos;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Infrastructure.DbContexts.Write;

namespace PocketCounter.Infrastructure.DbContexts.Read;

public class ReadDbContext(IConfiguration configuration) : DbContext, IReadDbContext
{
    public DbSet<CategoryDto> Categories => Set<CategoryDto>();
    
    public DbSet<ProductDto> Products => Set<ProductDto>();
    
    public DbSet<CustomerDto> Customers => Set<CustomerDto>();
    
    public DbSet<OrderDto> Orders => Set<OrderDto>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DB_CONNECTION_STRING));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<ProductDto>().HasQueryFilter(p => p.)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Read") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
}
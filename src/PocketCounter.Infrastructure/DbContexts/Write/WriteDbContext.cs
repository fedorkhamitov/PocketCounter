using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;

namespace PocketCounter.Infrastructure.DbContexts.Write;

public class WriteDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Category> Categories { get; init; }
    public DbSet<Customer> Customers { get; init; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString(Constants.DB_CONNECTION_STRING));
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseLoggerFactory(CreateLoggerFactory());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WriteDbContext).Assembly,
            type => type.FullName?.Contains("Configurations.Write") ?? false);
    }

    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
}
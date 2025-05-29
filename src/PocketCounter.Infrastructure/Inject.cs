using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PocketCounter.Application.Categories;
using PocketCounter.Application.Customers;
using PocketCounter.Application.Database;
using PocketCounter.Infrastructure.DbContexts.Read;
using PocketCounter.Infrastructure.DbContexts.Write;
using PocketCounter.Infrastructure.Repositories;

namespace PocketCounter.Infrastructure;

public static class Inject
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<WriteDbContext>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IReadDbContext, ReadDbContext>();
        
        //services.AddMinio(configuration);
        return services;
    }
}
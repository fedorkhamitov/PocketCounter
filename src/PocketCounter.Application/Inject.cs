using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PocketCounter.Application.Authorization;
using PocketCounter.Application.Categories.Create;
using PocketCounter.Application.Categories.Delete;
using PocketCounter.Application.Categories.Products.Create;
using PocketCounter.Application.Categories.Products.Delete;
using PocketCounter.Application.Categories.Products.Update;
using PocketCounter.Application.Categories.Queries.GetCategoriesList;
using PocketCounter.Application.Categories.Queries.GetCategoriesWithPagination;
using PocketCounter.Application.Categories.Update;
using PocketCounter.Application.Customers.Create;
using PocketCounter.Application.Customers.Delete;
using PocketCounter.Application.Customers.Orders.Create;
using PocketCounter.Application.Customers.Orders.Delete;
using PocketCounter.Application.Customers.Orders.Update;
using PocketCounter.Application.Customers.Queries;
using PocketCounter.Application.Customers.Queries.GetCustomersList;
using PocketCounter.Application.Customers.Update;

namespace PocketCounter.Application;

public static class Inject
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        /* --- Handlers --- */
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<DeleteCategoryHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<UpdateCategoryHandler>();
        services.AddScoped<UpdateProductMainInfoHandler>();
        services.AddScoped<UpdateProductPriceHandler>();
        services.AddScoped<UpdateProductDimensionsHandler>();
        services.AddScoped<UpdateProductQuantityHandler>();
        services.AddScoped<GetCategoriesWithPaginationHandler>();
        services.AddScoped<CreateCustomerHandler>();
        services.AddScoped<DeleteCustomerHandler>();
        services.AddScoped<UpdateCustomerHandler>();
        services.AddScoped<CreateOrderHandler>();
        services.AddScoped<DeleteOrderHandler>();
        services.AddScoped<UpdateOrderCartLinesHandler>();
        services.AddScoped<UpdateOrderStatusHandler>();
        services.AddScoped<GetCustomersWithPaginationHandler>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<GetCategoriesListHandler>();
        services.AddScoped<GetCustomersListHandler>();
        services.AddScoped<UpdateOrderAddressHandler>();
        
        services.AddValidatorsFromAssembly(typeof(Inject).Assembly);
        return services;
    }
}
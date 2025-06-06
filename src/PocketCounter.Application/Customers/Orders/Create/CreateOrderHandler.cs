using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Application.Categories;
using PocketCounter.Application.Database;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Orders.Create;

public class CreateOrderHandler(
    ICustomerRepository customerRepository,
    ICategoryRepository categoryRepository,
    IReadDbContext readDbContext,
    ILogger<CreateOrderHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid id,
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetById(id, cancellationToken);

        var addressResult = Address.Create(
            request.Address.ZipCode,
            request.Address.Country,
            request.Address.State,
            request.Address.City,
            request.Address.StreetName,
            request.Address.StreetNumber,
            request.Address.Apartment,
            request.Address.SpecialAddressString);
        if (addressResult.IsFailure)
            return addressResult.Error;

        var cartLines = request.CartLineDtos
            .Select(dto => CartLine.Create(dto.ProductId, dto.Quantity))
            .Where(result => result.IsSuccess)
            .Select(result => result.Value)
            .ToList();

        decimal totalPrice = 0;
        
        foreach (var cartLine in cartLines)
        {
            var product = await categoryRepository.GetProductById(cartLine.ProductId, cancellationToken);
            if (product.IsFailure)
                return Errors.General.NotFound(cartLine.ProductId);
            totalPrice += product.Value.GetTotalPriceForOrderQuantity(cartLine.Quantity).Value;
        }

        var ordersCount = readDbContext.Orders.Count();
        
        var serialNumber = SerialNumber.Create(ordersCount + 1);
        if (serialNumber.IsFailure)
            return serialNumber.Error;
        
        var order = new Order(
            cartLines,
            serialNumber.Value,
            addressResult.Value,
            totalPrice,
            request.Comment);

        customer.AddOrder(order);

        await customerRepository.Save(customer, cancellationToken);

        return order.Id;
    }
}
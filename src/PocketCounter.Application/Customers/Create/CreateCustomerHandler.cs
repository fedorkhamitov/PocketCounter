using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Entities;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Create;

public class CreateCustomerHandler(ICustomerRepository categoryRepository, ILogger<CreateCustomerHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var fullName = HumanName.Create(
            request.FullName.FirstName,
            request.FullName.FamilyName,
            request.FullName.Patronymic);
        if (fullName.IsFailure)
            return fullName.Error;

        Customer customer;
        
        if (request.PhoneNumber != "")
        {
            var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
            if (phoneNumber.IsFailure)
                return phoneNumber.Error;
            
            customer = new Customer(fullName.Value, phoneNumber.Value);
        }
        else
        {
            customer = new Customer(fullName.Value, PhoneNumber.Create(string.Empty).Value);
        }
        
        var result = await categoryRepository.Add(customer, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        logger.LogInformation("Created new Category Id: {0}, Name: {1}", customer.Id, customer.Name);

        return customer.Id;
    }
}
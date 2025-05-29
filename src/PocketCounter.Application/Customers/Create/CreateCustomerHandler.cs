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
            request.FullName.Patronymic,
            request.FullName.FamilyName);
        if (fullName.IsFailure)
            return fullName.Error;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumber.IsFailure)
            return phoneNumber.Error;
        var customer = new Customer(fullName.Value, phoneNumber.Value);
       
        var result = await categoryRepository.Add(customer, cancellationToken);
        if (result.IsFailure)
            return result.Error;
        
        logger.LogInformation("Created new Category Id: {0}, Name: {1}", customer.Id, customer.Name);

        return customer.Id;
    }
}
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using PocketCounter.Domain.Share;
using PocketCounter.Domain.ValueObjects;

namespace PocketCounter.Application.Customers.Update;

public class UpdateCustomerHandler(ICustomerRepository customerRepository, ILogger<UpdateCustomerHandler> logger)
{
    public async Task<Result<Guid, Error>> Handle(
        Guid id,
        UpdateCustomerRequest request, 
        CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetById(id, cancellationToken);

        var fullName = HumanName.Create(
            request.FullName.FirstName,
            request.FullName.FamilyName,
            request.FullName.Patronymic);
        if (fullName.IsFailure)
            return fullName.Error;

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);
        if (phoneNumber.IsFailure)
            return phoneNumber.Error;
        
        var customerUpdated = customer.Update(fullName.Value, phoneNumber.Value);
        if (customerUpdated.IsFailure)
            return customerUpdated.Error;
        
        var result = await customerRepository.Save(customer, cancellationToken);
        
        logger.LogInformation("Updated Category with Id: {0}, Name: {1}", 
            customer.Id, customer.Name);

        return result;
    }
}
using PocketCounter.Application.Dtos;

namespace PocketCounter.Application.Customers.Create;

public record CreateCustomerRequest(HumanNameDto FullName, string PhoneNumber);
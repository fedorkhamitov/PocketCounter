using PocketCounter.Application.Dtos;

namespace PocketCounter.Application.Customers.Update;

public record UpdateCustomerRequest(HumanNameDto FullName, string PhoneNumber);
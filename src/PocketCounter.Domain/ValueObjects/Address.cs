using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using PocketCounter.Domain.Share;

namespace PocketCounter.Domain.ValueObjects;

public record Address
{
    public string ZipCode { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string City { get; } = default!;
    public string StreetName { get; } = default!;
    public string StreetNumber { get; } = default!;
    public string Apartment { get; } = default!;
    public string SpecialAddressString { get; } = default!;

    private Address()
    {
    }

    [JsonConstructor]
    private Address(
        string zipCode,
        string country,
        string state,
        string city,
        string streetName,
        string streetNumber,
        string apartment,
        string specialAddressString = ""
    )
    {
        ZipCode = zipCode;
        Country = country;
        State = state;
        City = city;
        StreetName = streetName;
        StreetNumber = streetNumber;
        Apartment = apartment;
        SpecialAddressString = specialAddressString;
    }

    public static Result<Address, Error> Create(
        string zipCode,
        string country,
        string state,
        string city,
        string streetName = "",
        string streetNumber = "",
        string apartment = "",
        string specialAddressString = ""
    )
    {
        if (string.IsNullOrWhiteSpace(streetName) &&
            string.IsNullOrWhiteSpace(streetNumber) &&
            string.IsNullOrWhiteSpace(apartment) &&
            string.IsNullOrWhiteSpace(specialAddressString))
            return Errors.General.ValueIsRequired("Special Address String with ect. props");
        
        return new Address(
            zipCode,
            country,
            state,
            city,
            streetName,
            streetNumber,
            apartment,
            specialAddressString
        );
    }
}
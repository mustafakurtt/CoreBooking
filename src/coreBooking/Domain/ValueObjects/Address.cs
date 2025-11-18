using Shared.Constants;
namespace Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }
    public string ZipCode { get; private set; }

    private Address() { }

    public Address(string street, string city, string country, string zipCode)
    {
        // Street Validation
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldEmpty, nameof(Street)));
        if (street.Length > EntityLengths.AddressLine)
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldTooLong, nameof(Street), EntityLengths.AddressLine));

        // City Validation
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldEmpty, nameof(City)));
        if (city.Length > EntityLengths.City)
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldTooLong, nameof(City), EntityLengths.City));

        // Country Validation
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldEmpty, nameof(Country)));
        if (country.Length > EntityLengths.City) // Using City length constant for Country as well
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldTooLong, nameof(Country), EntityLengths.City));

        // ZipCode Validation
        if (string.IsNullOrWhiteSpace(zipCode))
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldEmpty, nameof(ZipCode)));
        if (zipCode.Length > EntityLengths.ZipCode)
            throw new ArgumentException(string.Format(ExceptionMessages.AddressFieldTooLong, nameof(ZipCode), EntityLengths.ZipCode));

        Street = street;
        City = city;
        Country = country;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return Country;
        yield return ZipCode;
    }

    public override string ToString() => $"{Street}, {City}, {Country} ({ZipCode})";
}
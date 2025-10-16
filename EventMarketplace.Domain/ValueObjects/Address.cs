using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Domain.ValueObjects;

public record Address
{
    public string? City { get; }
    public string? Street { get; }
    public string? Number { get;  }
    public string? PostalCode { get; }

    private Address(string street, string city, string number, string postalCode)
    {
        Street = street;
        City = city;
        Number = number;
        PostalCode = postalCode;
    }

    public static Address Create(string street, string city, string number, string postalCode)
    {
        if (string.IsNullOrEmpty(street))
            throw new Exception("Ulica nie może być pusta.");
        if (string.IsNullOrEmpty(city))
            throw new Exception("Miasto nie może być puste.");
        if (string.IsNullOrEmpty(number))
            throw new Exception("Numer nie może być pusty.");
        if (string.IsNullOrEmpty(postalCode))
            throw new Exception("Kod pocztowy nie może być pusty.");

        return new Address(street.Trim(), city.Trim(), number.Trim(), postalCode.Trim());
    }

    public override string ToString()
    {
        return $"{Street}, {Number}, {PostalCode}, {City}";
    }
}
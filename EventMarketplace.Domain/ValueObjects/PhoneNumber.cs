using System.Text.RegularExpressions;

namespace EventMarketplace.Domain.ValueObjects;

public record PhoneNumber
{
    private static readonly Regex PhonePattern = new Regex(@"^\+?\d{1,3}?[-.\s()]?\(?\d{1,4}\)?[-.\s()]?\d{1,4}[-.\s()]?\d{1,9}$");
    public string PhoneValue { get; }

    private PhoneNumber(string phoneValue)
    {
        PhoneValue = phoneValue;
    }

    public static PhoneNumber Create(string phoneValue)
    {
        if (string.IsNullOrEmpty(phoneValue))
            throw new Exception("Numer telefonu nie może być pusty.");
        if (!PhonePattern.IsMatch(phoneValue))
            throw new Exception("Podany numer telefonu jest nieprawidłowy.");
            
        return new PhoneNumber(phoneValue);
    }

    public override string ToString()
    {
        return PhoneValue;
    }
}
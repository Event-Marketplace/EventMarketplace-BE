using System.Text.RegularExpressions;

namespace EventMarketplace.Domain.ValueObjects;

public record EmailAddress
{
    private static readonly Regex EmailPattern = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    
    public string Value { get; }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static EmailAddress Create(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new ArgumentException("Email nie może być pusty.");
  

        email = email.Trim().ToLowerInvariant();

        if (!EmailPattern.IsMatch(email))
            throw new ArgumentException("Nieprawidłowy adres email.");

        return new EmailAddress(email);
    }

    public override string ToString()
    {
        return Value;
    }
}
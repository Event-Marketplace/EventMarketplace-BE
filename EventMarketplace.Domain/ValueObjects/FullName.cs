namespace EventMarketplace.Domain.ValueObjects;

public record FullName
{
    public string FirstName { get; }
    public string LastName { get; }

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static FullName Create(string firstName, string lastName)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new Exception("Imię nie może być puste.");
        if (string.IsNullOrEmpty(lastName))
            throw new Exception("Nazwisko nie może być puste.");
        if (firstName.Equals(lastName))
            throw new Exception("Imię i nazwisko nie mogą być takie same.");
        
        return new FullName(firstName, lastName);
    }

    public override string ToString()
    {
        return $"{FirstName} {LastName}";
    }
}
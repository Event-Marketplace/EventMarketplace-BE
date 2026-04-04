namespace EventMarketplace.Application.Response.UserResponse;

public class GetUserInfoResponse
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public string? Number { get; set; }
    public DateTime RegistrationDate { get; set; }
}
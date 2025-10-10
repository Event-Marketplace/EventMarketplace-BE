using EventMarketplace.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EventMarketplace.Domain.Entities;

public class User : BaseEntity
{
    //Data
    public FullName FullName { get; set; }
    public EmailAddress EmailAddress { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
    public string Password { get; set; }
    public bool IsOrganizerAccount { get; set; }
    public Address Address { get; set; }
}
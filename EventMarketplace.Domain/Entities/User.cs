using System.Collections.ObjectModel;
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

    public ICollection<Event> Events { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
}
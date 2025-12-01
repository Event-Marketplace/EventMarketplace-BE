using System.Collections.ObjectModel;
using System.Data.Common;
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
    public static User CreateUser(string email)
    {
        return new User()
        {
            EmailAddress = EmailAddress.Create(email),
            CreateAt = DateTime.UtcNow,
        };
    }

    public void AssignRole(Role role)
    {
        if (UserRoles.Any(x => x.RoleId == role.Id)) 
            throw new ApplicationException("Given role is already added to user.");
        
        UserRoles.Add(UserRole.Create(this, role));
    }

    public void SetPassword(string hashedPassword)
    {
        Password = hashedPassword;
    }
}
namespace EventMarketplace.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Value { get; set; }
    public DateTime Expires { get; set; }
    public bool Revoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string? ReplacedBy { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public static RefreshToken Create(string value, DateTime expires, bool revoked, Guid userId)
    {
        return new RefreshToken()
        {
            Value = value,
            Expires = expires,
            Revoked = false,
            CreateAt = DateTime.UtcNow,
            UserId = userId
        };
    }

    public void SetRevokedToken()
    {
        Revoked = true;
        RevokedAt = DateTime.UtcNow;
    }
}
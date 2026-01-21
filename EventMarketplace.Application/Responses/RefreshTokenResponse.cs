namespace EventMarketplace.Application.Response;

public class RefreshTokenResponse
{
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}
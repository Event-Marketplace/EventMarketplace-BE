namespace EventMarketplace.Application.Dtos.UserDtos;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public bool IsOrganizerAccount { get; set; }
}
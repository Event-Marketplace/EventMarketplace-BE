namespace EventMarketplace.Application.Utils;

public interface IPasswordManager
{
    string HashPassword(string password);
    bool ValidPassword(string password, string hashedPassword);
}
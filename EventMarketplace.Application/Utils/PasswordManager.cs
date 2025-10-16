using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;

namespace EventMarketplace.Application.Utils;

public class PasswordManager : IPasswordManager
{
    public string HashPassword(string password)
    {
        ValidatePassword(password);
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool ValidPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            throw new Exception("Hasło musi mieć minimum 8 znaków.");

        if (!Regex.IsMatch(password, @"[A-Z]"))
            throw new Exception("Hasło musi zawierać co najmniej jedną dużą literę.");

        if (!Regex.IsMatch(password, @"[0-9]"))
            throw new Exception("Hasło musi zawierać co najmniej jedną cyfrę.");

        if (!Regex.IsMatch(password, @"[\W_]"))
            throw new Exception("Hasło musi zawierać co najmniej jeden znak specjalny.");
    }
}
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace Service.Utils;

public static class PasswordHelper
{
    public static string GenerateRandomPassword(int length = 18)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,.<>?";
        return RandomNumberGenerator.GetString(validChars, length);
    }
}
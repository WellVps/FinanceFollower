using System.Security.Cryptography;
using System.Text;
using Service.Services.Authentication.Interfaces;

namespace Service.Services.Authentication;

public class Md5PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var valueAsBytes = Encoding.UTF8.GetBytes(password);
        var hashedBytes = MD5.HashData(valueAsBytes);

        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        return HashPassword(password) == hashedPassword;
    }
}
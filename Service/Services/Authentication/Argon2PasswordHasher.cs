using Isopoh.Cryptography.Argon2;
using Service.Services.Authentication.Interfaces;

namespace Service.Services.Authentication;

public sealed class Argon2PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return Argon2.Hash(
            password: password,
            memoryCost: 19456,
            timeCost: 2,
            parallelism: 1
        );
    }

    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        return Argon2.Verify(encoded: hashedPassword, password: password);
    }
}
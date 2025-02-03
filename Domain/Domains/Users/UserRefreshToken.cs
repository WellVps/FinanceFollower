using BaseDomain;

namespace Domain.Domains.Users;

public class UserRefreshToken : BaseEntity
{
    public string UserId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }

    public UserRefreshToken(string userId, string refreshToken, DateTime expiration)
    {
        UserId = userId;
        RefreshToken = refreshToken;
        Expiration = expiration;
    }

    protected override void ValidateRules()
    {
        if (string.IsNullOrWhiteSpace(UserId))
        {
            throw new ArgumentNullException(nameof(UserId), "UserId is required");
        }

        if (string.IsNullOrWhiteSpace(RefreshToken))
        {
            throw new ArgumentNullException(nameof(RefreshToken), "RefreshToken is required");
        }

        if (Expiration == default)
        {
            throw new ArgumentNullException(nameof(Expiration), "Expiration is required");
        }
    }
}
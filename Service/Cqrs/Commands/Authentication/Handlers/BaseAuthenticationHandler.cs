using Service.Utils;
using BaseApi.Auth.Dtos;
using Domain.Domains.Users;
using System.Security.Cryptography;
using Infraestructure.Repositories.Interfaces;
using Service.Cqrs.Commands.Authentication.Responses;

namespace Service.Cqrs.Commands.Authentication.Handlers;

public class BaseAuthenticationHandler(IUserRefreshTokenRepository userRefreshTokenRepository)
{
    protected readonly IUserRefreshTokenRepository _userRefreshTokenRepository = userRefreshTokenRepository;

    protected async Task<AuthenticationResponse?> CreateAuthenticationResponse(User? user, 
        TokenConfiguration tokenConfigs, string? refreshTokenToMaintain = null,
        CancellationToken cancellationToken = default
    )
    {
        if (user == null)
        {
            return default;
        }

        Validations(user, tokenConfigs);

        var (token, expiration) = JwtUserAuth.GenerateToken(
            user,
            tokenConfigs.Key!,
            tokenConfigs.ValidityToken
        );

        var refreshToken = refreshTokenToMaintain ?? await GenerateRefreshToken(user.Id, tokenConfigs, cancellationToken);

        return new AuthenticationResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpirationDate = expiration
        };
    }

    private void Validations(User? user, TokenConfiguration? tokenConfiguration)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        if (tokenConfiguration == null || string.IsNullOrWhiteSpace(tokenConfiguration.Key))
        {
            throw new ArgumentNullException(nameof(tokenConfiguration));
        }
    }

    private async Task<string> GenerateRefreshToken(string userId, TokenConfiguration tokenConfigs, CancellationToken cancellationToken)
    {
        var randomNumber = new byte[64];
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshToken = Convert.ToBase64String(randomNumber);

        var expiration = DateTime.UtcNow.AddDays(tokenConfigs.ValidityRefreshToken);

        var hasToken = await _userRefreshTokenRepository.HasRecord(x => x.UserId == userId, cancellationToken);
        if (hasToken)
        {
            await _userRefreshTokenRepository.Update(
                x => x.UserId == userId, 
                cancellationToken, 
                (x => x.RefreshToken, refreshToken), 
                (x => x.Expiration, expiration)
            );

            return refreshToken;
        }

        await _userRefreshTokenRepository.Save(new UserRefreshToken(userId, refreshToken, expiration), cancellationToken);

        return refreshToken;
    }
}
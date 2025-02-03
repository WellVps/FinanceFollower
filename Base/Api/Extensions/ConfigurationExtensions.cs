using BaseApi.Auth.Dtos;
using BaseApi.Utils;
using BaseInfraestructure.Messaging.Models;
using Microsoft.Extensions.Configuration;

namespace BaseApi.Extensions;

public static class ConfigurationExtensions
{
    public static RabbitMQConfig GetRabbitMQConfiguration(this IConfiguration configuration)
    {
        _ = int.TryParse(configuration[ConfigurationKeys.RabbitMQPort], out int port);
        _ = bool.TryParse(configuration[ConfigurationKeys.RabbitMQActive], out bool active);

        var config = new RabbitMQConfig
        {
            Active = active,
            HostName = configuration[ConfigurationKeys.RabbitMQHost] ?? string.Empty,
            Password = configuration[ConfigurationKeys.RabbitMQPassword] ?? string.Empty,
            UserName = configuration[ConfigurationKeys.RabbitMQUserName] ?? string.Empty,
            Port = port
        };

        return config;
    }

    public static TokenConfiguration GetUserTokenConfiguration(this IConfiguration configuration)
    {
        _ = int.TryParse(configuration[ConfigurationKeys.UserJwtValidityInDays], out int validityToken);
        _ = int.TryParse(configuration[ConfigurationKeys.UserJwtRefreshTokenValidityInDays], out int validityRefreshToken);

        var configDto = new TokenConfiguration
        {
            Key = configuration[ConfigurationKeys.UserJwtKey] ?? string.Empty,
            ValidityToken = validityToken,
            ValidityRefreshToken = validityRefreshToken
        };

        return configDto;
    }
}
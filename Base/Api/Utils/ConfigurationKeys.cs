namespace BaseApi.Utils;

public static class ConfigurationKeys
{
    public const string UserJwtKey = "Keys:SecretJwtKey:Key";
    public const string UserJwtValidityInDays = "Keys:SecretJwtKey:TokenValidityInDays";
    public const string UserJwtRefreshTokenValidityInDays = "Keys:SecretJwtKey:RefreshTokenValidityInDays";

    public const string RabbitMQActive = "RabbitMQ:Active";
    public const string RabbitMQHost = "RabbitMQ:Host";
    public const string RabbitMQPort = "RabbitMQ:Port";
    public const string RabbitMQUserName = "RabbitMQ:UserName";
    public const string RabbitMQPassword = "RabbitMQ:Password";
}
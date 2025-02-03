namespace BaseInfraestructure.Messaging.Models;

public class RabbitMQConfig
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConnectionName { get; set; }
    public bool Active { get; set; }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(HostName) &&
        !string.IsNullOrWhiteSpace(UserName) &&
        !string.IsNullOrWhiteSpace(Password) &&
        Port > 0;
}
using RabbitMQ.Client;

namespace BaseInfraestructure.Messaging.Interfaces;

public interface IRabbitMQConnection
{
    bool IsConnected { get; }
    bool TryConnect();
    IModel CreateModel();
}
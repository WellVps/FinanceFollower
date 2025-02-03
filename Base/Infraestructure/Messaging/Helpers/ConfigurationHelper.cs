using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using BaseInfraestructure.Messaging.Models;
using BaseInfraestructure.Messaging.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BaseInfraestructure.Messaging.Helpers;

public static class ConfigurationHelper
{
    public static void RegisterRabbitMQConnections(IServiceCollection services, RabbitMQConfig configuration)
    {
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus, EventBus>(sp => {
            var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
            var rabbitMqConnection = sp.GetRequiredService<IRabbitMQConnection>();
            var logger = sp.GetRequiredService<ILogger<EventBus>>();

            return new EventBus(eventBusSubscriptionsManager, rabbitMqConnection, logger, sp);
        });

        services.AddSingleton<IRabbitMQConnection>(_ => {
            var connectionFactory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                UserName = configuration.UserName,
                Password = configuration.Password,
                Port = configuration.Port
            };

            return new RabbitMQConnection(connectionFactory, configuration.ConnectionName);
        });
    }
}
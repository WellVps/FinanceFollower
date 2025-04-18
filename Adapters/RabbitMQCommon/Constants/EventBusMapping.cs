using BaseInfraestructure.Messaging.Models;

namespace RabbitMQCommon.Constants;

public static class EventBusMapping
{
    public static readonly EventBusOptions UpdateLastPrice = EventBusOptions.Config(
        ExchangeMapping.DefaultExchange,
        QueueMapping.UpdateLastPrice,
        withDeadletter: false,
        prefetchCount: 10
    );

    public static readonly EventBusOptions UpdateAssets = EventBusOptions.Config(
        ExchangeMapping.DefaultExchange,
        QueueMapping.UpdateAssets,
        withDeadletter: false,
        prefetchCount: 10
    );
}
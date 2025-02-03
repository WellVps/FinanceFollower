using BaseInfraestructure.Messaging.Models;

namespace BaseInfraestructure.Messaging.Interfaces;
public interface IEventBus
{
    void Publish(Event @event, string queueName, string exchangeName);

    void Publish(Event @event, EventBusOptions options);

    void Subscribe<E, EH> (string queueName, string exchangeName, ushort? prefetchCount = 10, bool deadLetter = false)
        where E : Event
        where EH : IEventHandler<E>;

    void Subscribe<E, EH> (EventBusOptions options)
        where E : Event
        where EH : IEventHandler<E>;

    void PublishInExchange(Event @event, string exchangeName);

    void SubscribeInExchange<E, EH>(string queueName, string exchangeName, ushort? prefetchCount = 10)
        where E : Event
        where EH : IEventHandler<E>;

    void SubscribeWithDeadletter<E, EH>(EventBusOptions options)
        where E : Event
        where EH : IEventHandler<E>;
}
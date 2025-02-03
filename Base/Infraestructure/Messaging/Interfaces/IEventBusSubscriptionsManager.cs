using BaseInfraestructure.Messaging.Dtos;
using BaseInfraestructure.Messaging.Models;

namespace BaseInfraestructure.Messaging.Interfaces;

public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }

    event EventHandler<string> OnEventRemoved;

    void AddSubscription<T, TH>(string queue)
        where T : Event
        where TH : IEventHandler<T>;

    void RemoveSubscription<T, TH>()
        where T : Event
        where TH : IEventHandler<T>;

    bool HasSubscriptionsForEvent<T>() where T : Event;

    bool HasSubscriptionsForEvent(string eventName);

    Type? GetEventTypeByNameQueue(string eventName);

    void Clear();

    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event;

    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

    string GetEventKey<T>();
}
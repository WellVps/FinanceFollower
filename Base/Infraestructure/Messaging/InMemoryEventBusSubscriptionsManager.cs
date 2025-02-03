using BaseInfraestructure.Messaging.Dtos;
using BaseInfraestructure.Messaging.Interfaces;
using BaseInfraestructure.Messaging.Models;

namespace BaseInfraestructure.Messaging;

public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public event EventHandler<string>? OnEventRemoved;

    public InMemoryEventBusSubscriptionsManager()
    {
        _handlers = new Dictionary<string, List<SubscriptionInfo>>();
        _eventTypes = new List<Type>();
    }

    public bool IsEmpty => !_handlers.Keys.Any();

    public void Clear() => _handlers.Clear();

    public void AddSubscription<T, TH>(string queue)
        where T : Event
        where TH : IEventHandler<T>
    {
        GetEventKey<T>();
        DoAddSubscriptionByQueue(typeof(TH), typeof(T), queue, false);
        if (_eventTypes.Contains(typeof(T)))
            return;
        _eventTypes.Add(typeof(T));
    }

    public string GetEventKey<T>() => typeof(T).Name;

    public Type? GetEventTypeByName(string eventName) =>
        _eventTypes.SingleOrDefault(t => t.Name == eventName);

    public Type? GetEventTypeByNameQueue(string queueName) => _handlers
        .FirstOrDefault(c => c.Key == queueName).Value
        ?.FirstOrDefault()?.EventType;

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : Event =>
        GetHandlersForEvent(GetEventKey<T>());

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(
        string eventName)
    {
        return _handlers[eventName];
    }

    public bool HasSubscriptionsForEvent<T>() where T : Event => throw new NotImplementedException();

    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

    public void RemoveSubscription<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        SubscriptionInfo? subscriptionToRemove = FindSubscriptionToRemove<T, TH>();
        DoRemoveHandler(GetEventKey<T>(), subscriptionToRemove);
    }

    private void DoAddSubscriptionByQueue(
        Type handlerType,
        Type eventType,
        string queue,
        bool isDynamic)
    {
        if (!HasSubscriptionsForEvent(queue))
            _handlers.Add(queue, []);

        if (_handlers[queue].Exists(s => s.HandlerType == handlerType))
            throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{queue}'",
                nameof(handlerType));

        if (isDynamic)
            _handlers[queue].Add(SubscriptionInfo.Dynamic(handlerType));
        else
            _handlers[queue].Add(SubscriptionInfo.Typed(handlerType, eventType, queue));
    }

    private SubscriptionInfo? FindSubscriptionToRemove<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        return DoFindSubscriptionToRemove(GetEventKey<T>(), typeof(TH));
    }

    private SubscriptionInfo? DoFindSubscriptionToRemove(
        string eventName,
        Type handlerType)
    {
        return !HasSubscriptionsForEvent(eventName)
            ? null
            : _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
    }

    private void DoRemoveHandler(
        string eventName,
        SubscriptionInfo? subsToRemove)
    {
        if (subsToRemove == null)
            return;
        _handlers[eventName].Remove(subsToRemove);
        if (!_handlers[eventName].Any())
        {
            _handlers.Remove(eventName);
            Type? type = _eventTypes.SingleOrDefault(e => e.Name == eventName);
            if (type != null)
                _eventTypes.Remove(type);
            RaiseOnEventRemoved(eventName);
        }
    }

    private void RaiseOnEventRemoved(string eventName)
    {
        if (OnEventRemoved == null)
            return;
        OnEventRemoved(this, eventName);
    }
}
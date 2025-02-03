namespace BaseInfraestructure.Messaging.Dtos;

public class SubscriptionInfo
{
    public bool IsDynamic { get; }

    public Type HandlerType { get; }

    public Type? EventType { get; }

    public string? QueueName { get; }

    private SubscriptionInfo(bool isDynamic, Type handlerType, Type? eventType = default, string? queueName = default)
    {
        IsDynamic = isDynamic;
        HandlerType = handlerType;
        EventType = eventType;
        QueueName = queueName;
    }

    public static SubscriptionInfo Dynamic(Type handlerType) => new (true, handlerType);

    public static SubscriptionInfo Typed(Type handlerType) => new(false, handlerType);

    public static SubscriptionInfo Typed(Type handlerType, Type eventType, string queueName) => new(false, handlerType, eventType, queueName);
}
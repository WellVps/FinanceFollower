namespace BaseInfraestructure.Messaging.Models;

public sealed record EventBusOptions
{
    public string ExchangeName { get; private init; } = string.Empty;

    public string QueueName { get; private init; } = string.Empty;

    public ushort? PrefetchCount { get; private init; } = 10;

    public bool WithDeadletter { get; private init; }

    public ushort? TimeToLive { get; private init; }

    public ushort? RequeueCount { get; private init; } = 3;

    public bool WithRequeue { get; private init; }

    public ushort? TimeToRequeue { get; private init; }

    public int? PriorityQueue { get; private init; }
    
    public string RequeueExchangeName => "requeue-exchange";

    public string RequeueQueueName => QueueName + ".requeue";

    public string DeadLetterExchangeName => "dead-letter-exchange";

    public string DeadLetterQueueName => QueueName + ".dead-letter";

    private EventBusOptions() { }
    
    public static EventBusOptions Config(
        string exchangeName,
        string queueName,
        bool withDeadletter,
        ushort? prefetchCount = 10,
        int? priorityQueue = null)
    {
        return new EventBusOptions
        {
            ExchangeName = exchangeName,
            QueueName = queueName,
            WithDeadletter = withDeadletter,
            PrefetchCount = prefetchCount,
            PriorityQueue = priorityQueue
        };
    }

    public static EventBusOptions ConfigWithDeadLetterWithDiscard(
        string exchangeName,
        string queueName,
        ushort timeToLiveInMinutes = 14400,
        ushort? prefetchCount = 10)
    {
        return new EventBusOptions
        {
            ExchangeName = exchangeName,
            QueueName = queueName,
            TimeToLive = timeToLiveInMinutes,
            PrefetchCount = prefetchCount,
            WithDeadletter = true
        };
    }

    public static EventBusOptions ConfigWithRequeueWithDeadLetterWithDiscard(
        string exchangeName,
        string queueName,
        ushort timeToLiveInMinutes = 14400,
        ushort requeueCount = 3,
        ushort timeToRequeueInMinutes = 5,
        ushort? prefetchCount = 10)
    {
        return new EventBusOptions
        {
            ExchangeName = exchangeName,
            QueueName = queueName,
            WithDeadletter = true,
            TimeToLive = timeToLiveInMinutes,
            WithRequeue = true,
            RequeueCount = requeueCount,
            TimeToRequeue = timeToRequeueInMinutes,
            PrefetchCount = prefetchCount
        };
    }
}
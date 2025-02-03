using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using BaseInfraestructure.Messaging.Exceptions;
using BaseInfraestructure.Messaging.Interfaces;
using BaseInfraestructure.Messaging.Models;

namespace BaseInfraestructure.Messaging;

public sealed class EventBus : IEventBus, IDisposable
{
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IRabbitMQConnection _rabbitConnection;
    private readonly ILogger<EventBus> _logger;
    private IModel _consumerChannel;
    private readonly IServiceProvider _serviceProvider;

    public EventBus(
        IEventBusSubscriptionsManager subsManager,
        IRabbitMQConnection rabbitConnection,
        ILogger<EventBus> logger,
        IServiceProvider serviceProvider)
    {
        _subsManager = subsManager;
        _rabbitConnection = rabbitConnection;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void Publish(Event @event, string queueName, string exchangeName)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        RetryPolicy policyRetry = CreatePolicyRetry();
        using (IModel channel = _rabbitConnection.CreateModel())
        {
            DeclareExchangeDirect(channel, exchangeName);
            DeclareQueue(channel, queueName, null);
            BindQueue(channel, exchangeName, queueName, queueName);
            policyRetry.Execute((Action)(() => PublishMessage(channel, @event, queueName, exchangeName)));
        }
    }

    public void Publish(Event @event, EventBusOptions options)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        RetryPolicy policyRetry = CreatePolicyRetry();
        using (IModel channel = _rabbitConnection.CreateModel())
        {
            Dictionary<string, object> arguments = GetArguments(channel, options);
            DeclareExchangeDirect(channel, options.ExchangeName);
            DeclareQueue(channel, options.QueueName, arguments);
            BindQueue(channel, options.ExchangeName, options.QueueName, options.QueueName);
            policyRetry.Execute(
                (Action)(() => PublishMessage(channel, @event, options.QueueName, options.ExchangeName)));
        }
    }

    public void PublishInExchange(Event @event, string exchangeName)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        RetryPolicy policyRetry = CreatePolicyRetry();
        using (IModel channel = _rabbitConnection.CreateModel())
        {
            DeclareExchangeFanOut(channel, exchangeName);
            policyRetry.Execute((Action)(() => PublishMessageInExchange(channel, @event, exchangeName)));
        }
    }

    public bool HasMessage(string queueName)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        using (IModel model = _rabbitConnection.CreateModel())
            return model.BasicGet(queueName, false).MessageCount > 0U;
    }

    public void Subscribe<E, EH>(
        string queueName,
        string exchangeName,
        ushort? prefetchCount = 10,
        bool deadLetter = false)
        where E : Event
        where EH : IEventHandler<E>
    {
        _subsManager.GetEventKey<E>();
        _subsManager.AddSubscription<E, EH>(queueName);
        DoInternalSubscriptionByQueue(queueName, exchangeName, prefetchCount, deadLetter);
    }

    public void Subscribe<E, EH>(EventBusOptions options)
        where E : Event
        where EH : IEventHandler<E>
    {
        _subsManager.GetEventKey<E>();
        _subsManager.AddSubscription<E, EH>(options.QueueName);
        DoInternalSubscriptionByQueue(options.QueueName, options.ExchangeName, options.PrefetchCount,
            options.WithDeadletter);
    }


    public void SubscribeInExchange<E, EH>(
        string queueName,
        string exchangeName,
        ushort? prefetchCount = 10)
        where E : Event
        where EH : IEventHandler<E>
    {
        _subsManager.AddSubscription<E, EH>(exchangeName);
        DoInternalSubscriptionByExchange(queueName, exchangeName, prefetchCount);
    }

    public void SubscribeWithDeadletter<E, EH>(EventBusOptions options)
        where E : Event
        where EH : IEventHandler<E>
    {
        _subsManager.AddSubscription<E, EH>(options.QueueName);
        CreateRabbitMQListenerDeadletter(options);
    }

    private void DoInternalSubscriptionByQueue(
        string queueName,
        string exchangeName,
        ushort? prefetchCount,
        bool deadLetter)
    {
        if (!_subsManager.HasSubscriptionsForEvent(queueName))
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();
            using (IModel model = _rabbitConnection.CreateModel())
                BindQueue(model, exchangeName, queueName, queueName);
        }

        CreateRabbitMQListener(queueName, exchangeName, prefetchCount, deadLetter);
    }

    private void DoInternalSubscriptionByExchange(
        string queueName,
        string exchangeName,
        ushort? prefetchCount)
    {
        if (!_subsManager.HasSubscriptionsForEvent(exchangeName))
        {
            if (!_rabbitConnection.IsConnected)
                _rabbitConnection.TryConnect();
            using (IModel model = _rabbitConnection.CreateModel())
            {
                DeclareExchangeFanOut(model, exchangeName);
                BindQueue(model, exchangeName, queueName, queueName);
            }
        }

        CreateRabbitMQListenerExchange(queueName, exchangeName, prefetchCount);
    }

    private async Task<bool> ProcessEvent(string eventName, string message)
    {
        try
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName).First();
                using (var scope = _serviceProvider.CreateScope())
                {
                    var handler = scope.ServiceProvider.GetService(subscriptions.HandlerType);

                    Type? eventType = _subsManager.GetEventTypeByNameQueue(eventName);
                    if (eventType is null) throw EventNameNotFoundException.OfEvent(eventName);

                    object integrationEvent = JsonConvert.DeserializeObject(message, eventType)!;
                    Type concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    
                    // call handler.Handle(integrationEvent)
                    bool flag = await (Task<bool>)concreteType
                        .GetMethod("Handle")!
                        .Invoke(handler, [integrationEvent])!;
                    return flag;
                }
            }
        }
        catch (JsonReaderException ex)
        {
            _logger.LogCritical(ex, "Erro ao converter deserializar Event - {EventName} \n Mensagem - {Message}",
                eventName, message);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Erro ao processar evento na camada Generics.Bus");
            return false;
        }

        return false;
    }

    private IModel CreateRabbitMQListener(
        string queueName,
        string exchangeName,
        ushort? prefetchCount,
        bool deadLetter = false)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        IModel channel = _rabbitConnection.CreateModel();
        if (prefetchCount.HasValue)
            channel.BasicQos(0U, prefetchCount.Value, false);
        Dictionary<string, object> arguments = new Dictionary<string, object>();
        if (deadLetter)
            arguments = CreateDeadLetterQueue(channel,
                EventBusOptions.Config(exchangeName, queueName, deadLetter, prefetchCount));
        DeclareExchangeDirect(channel, exchangeName);
        DeclareQueue(channel, queueName, arguments);
        BindQueue(channel, exchangeName, queueName, queueName);
        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
        consumer.Received += (EventHandler<BasicDeliverEventArgs>)(async (_, ea) =>
        {
            string eventName = ea.RoutingKey;
            ReadOnlyMemory<byte> body = ea.Body;
            string message = Encoding.UTF8.GetString(body.Span);
            bool success = await ProcessEvent(eventName, message);
            if (success)
                channel.BasicAck(ea.DeliveryTag, false);
            if (!success && deadLetter)
            {
                channel.BasicNack(ea.DeliveryTag, false, false);
                _logger.LogCritical("Queue: {QueueName} - Message: {Message}", queueName, message);
            }
        });
        DeclareConsumer(channel, consumer, queueName);
        channel.CallbackException += (EventHandler<CallbackExceptionEventArgs>)((_, _) =>
        {
            _consumerChannel.Dispose();
            _consumerChannel = CreateRabbitMQListener(queueName, exchangeName, prefetchCount);
        });
        return channel;
    }

    private IModel CreateRabbitMQListenerDeadletter(EventBusOptions options)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        IModel channel = _rabbitConnection.CreateModel();
        ushort? prefetchCount1 = options.PrefetchCount;
        if (prefetchCount1.HasValue)
        {
            IModel model = channel;
            int prefetchCount2 = prefetchCount1.Value;
            model.BasicQos(0U, (ushort)prefetchCount2, false);
        }

        Dictionary<string, object> arguments = GetArguments(channel, options);
        DeclareExchangeDirect(channel, options.ExchangeName);
        DeclareQueue(channel, options.QueueName, arguments);
        BindQueue(channel, options.ExchangeName, options.QueueName, options.QueueName);
        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
        consumer.Received += (EventHandler<BasicDeliverEventArgs>)(async (_, ea) =>
        {
            string eventName = ea.RoutingKey;
            ReadOnlyMemory<byte> body = ea.Body;
            string message = Encoding.UTF8.GetString(body.Span);
            bool success = await ProcessEvent(eventName, message);
            object? xdeath;
            if (ea.BasicProperties.Headers != null && ea.BasicProperties.Headers.TryGetValue("x-death", out xdeath))
            {
                List<object>? xDeathObjects = xdeath as List<object>;
                IDictionary<string, object>? countObject = xDeathObjects?[0] as IDictionary<string, object>;
                object? value = 0;
                if (countObject is not null) countObject.TryGetValue("count", out value);

                int count = Convert.ToInt32(value);
                int num = count;
                ushort? requeueCount = options.RequeueCount;
                int? nullable = requeueCount.HasValue ? requeueCount.GetValueOrDefault() : new int?();
                int valueOrDefault = nullable.GetValueOrDefault();
                if (num >= valueOrDefault && nullable.HasValue)
                {
                    PublishMessageDeadLetter(channel, message, options.DeadLetterQueueName,
                        options.DeadLetterExchangeName);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    if (success)
                        channel.BasicAck(ea.DeliveryTag, false);
                    if (!success && options.WithRequeue)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, false);
                        _logger.LogCritical("Queue: {QueueName} | Attempt: {Count} | Message: {Message}",
                            options.QueueName, count, message);
                    }
                }
            }
            else
            {
                if (success)
                    channel.BasicAck(ea.DeliveryTag, false);
                if (!success && options.WithDeadletter)
                {
                    channel.BasicNack(ea.DeliveryTag, false, false);
                    _logger.LogCritical("Queue: {QueueName} | Message: {Message} ", options.QueueName, message);
                }
            }
        });
        DeclareConsumer(channel, consumer, options.QueueName);
        channel.CallbackException += (EventHandler<CallbackExceptionEventArgs>)((_, _) =>
        {
            _consumerChannel.Dispose();
            _consumerChannel = CreateRabbitMQListenerDeadletter(options);
        });
        return channel;
    }

    private Dictionary<string, object> GetArguments(IModel channel, EventBusOptions options)
    {
        Dictionary<string, object> arguments = new Dictionary<string, object>();

        if (options.WithDeadletter)
            arguments = CreateDeadLetterQueue(channel, options);
        if (options.WithRequeue)
            arguments = CreateQueueForRequeue(channel, options);

        if (options.PriorityQueue is > 0 and < 6)
            arguments = AddPriorityArgument(arguments, options);

        return arguments;
    }

    private Dictionary<string, object> AddPriorityArgument(Dictionary<string, object> arguments,
        EventBusOptions options)
    {
        arguments.Add("x-max-priority", options.PriorityQueue!);
        return arguments;
    }

    private Dictionary<string, object> CreateQueueForRequeue(
        IModel channel,
        EventBusOptions options)
    {
        var arguments = new Dictionary<string, object>
        {
            { "x-dead-letter-exchange", options.ExchangeName },
            { "x-dead-letter-routing-key", options.QueueName },
            { "x-message-ttl", (int)TimeSpan.FromMinutes(options.TimeToRequeue.GetValueOrDefault()).TotalMilliseconds }
        };
        DeclareExchangeDirect(channel, options.RequeueExchangeName);
        DeclareQueue(channel, options.RequeueQueueName, arguments);
        BindQueue(channel, options.RequeueExchangeName, options.RequeueQueueName, options.RequeueQueueName);
        return new Dictionary<string, object>
        {
            {
                "x-dead-letter-exchange",
                options.RequeueExchangeName
            },
            {
                "x-dead-letter-routing-key",
                options.RequeueQueueName
            }
        };
    }

    private Dictionary<string, object> CreateDeadLetterQueue(
        IModel channel,
        EventBusOptions options)
    {
        Dictionary<string, object> arguments = new Dictionary<string, object>();
        if (options.TimeToLive.GetValueOrDefault() > 0)
            arguments.Add("x-message-ttl",
                (int)TimeSpan.FromMinutes(options.TimeToLive.GetValueOrDefault()).TotalMilliseconds);
        DeclareExchangeDirect(channel, options.DeadLetterExchangeName);
        DeclareQueue(channel, options.DeadLetterQueueName, arguments);
        BindQueue(channel, options.DeadLetterExchangeName, options.DeadLetterQueueName, options.DeadLetterQueueName);
        if (options.WithRequeue)
            return new();

        return new Dictionary<string, object>
        {
            {
                "x-dead-letter-exchange",
                options.DeadLetterExchangeName
            },
            {
                "x-dead-letter-routing-key",
                options.DeadLetterQueueName
            }
        };
    }

    private void CreateRabbitMQListenerExchange(string queueName,
        string exchangeName,
        ushort? prefetchCount)
    {
        if (!_rabbitConnection.IsConnected)
            _rabbitConnection.TryConnect();
        IModel channel = _rabbitConnection.CreateModel();
        if (prefetchCount.HasValue)
            channel.BasicQos(0U, prefetchCount.Value, false);
        DeclareExchangeFanOut(channel, exchangeName);
        DeclareQueue(channel, queueName, null);
        BindQueue(channel, exchangeName, queueName, queueName);
        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
        consumer.Received += (EventHandler<BasicDeliverEventArgs>)(async (_, ea) =>
        {
            string eventName = ea.RoutingKey;
            ReadOnlyMemory<byte> body = ea.Body;
            string message = Encoding.UTF8.GetString(body.Span);
            bool success = await ProcessEvent(eventName, message);
            if (success)
            {
                channel.BasicAck(ea.DeliveryTag, false);
            }
        });
        DeclareConsumer(channel, consumer, queueName);
        channel.CallbackException += (EventHandler<CallbackExceptionEventArgs>)((_, _) =>
        {
            _consumerChannel.Dispose();
            _consumerChannel = CreateRabbitMQListener(queueName, exchangeName, prefetchCount);
        });
    }

    private void PublishMessage(
        IModel channel,
        Event @event,
        string queueName,
        string exchangeName)
    {
        IBasicProperties basicProperties = channel.CreateBasicProperties();
        basicProperties.DeliveryMode = 2;
        basicProperties.ContentType = "application/json";
        basicProperties.Headers = GetHeaders();
        var json = JsonConvert.SerializeObject(@event);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchangeName, queueName, true, basicProperties, (ReadOnlyMemory<byte>)bytes);
    }

    private void PublishMessageInExchange(IModel channel, Event @event, string exchangeName)
    {
        var json = JsonConvert.SerializeObject(@event);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        IBasicProperties basicProperties = channel.CreateBasicProperties();
        basicProperties.DeliveryMode = 2;
        basicProperties.Persistent = true;
        basicProperties.ContentType = "application/json";
        basicProperties.Headers = GetHeaders();
        channel.BasicPublish(exchangeName, exchangeName, basicProperties, (ReadOnlyMemory<byte>)bytes);
    }

    private void PublishMessageDeadLetter(
        IModel channel,
        string message,
        string queueName,
        string exchangeName)
    {
        IBasicProperties basicProperties = channel.CreateBasicProperties();
        basicProperties.DeliveryMode = 2;
        basicProperties.ContentType = "application/json";
        basicProperties.Headers = GetHeaders();
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchangeName, queueName, true, basicProperties, (ReadOnlyMemory<byte>)bytes);
    }

    private IDictionary<string, object> GetHeaders() => new Dictionary<string, object>
    {
        {
            "produtor",
            ((RabbitMQConnection)_rabbitConnection).ClientProvidedName
        }
    };

    private static void DeclareExchangeDirect(IModel channel, string exchangeName) =>
        channel.ExchangeDeclare(exchangeName, "direct");

    private static void DeclareExchangeFanOut(IModel channel, string exchangeName) =>
        channel.ExchangeDeclare(exchangeName, "fanout", true);

    private static void DeclareQueue(
        IModel channel,
        string queueName,
        Dictionary<string, object>? arguments)
    {
        channel.QueueDeclare(queueName, true, false, false, arguments);
    }

    private static void BindQueue(
        IModel channel,
        string exchangeName,
        string queueName,
        string routingKey)
    {
        channel.QueueBind(queueName, exchangeName, routingKey, null);
    }

    private void DeclareConsumer(IModel channel, EventingBasicConsumer consumer, string queueName)
    {
        string clientProvideName = ((RabbitMQConnection)_rabbitConnection).ClientProvidedName;
        channel.BasicConsume(queueName, false, clientProvideName, consumer);
    }

    private static RetryPolicy CreatePolicyRetry() => Policy.Handle<BrokerUnreachableException>().Or<SocketException>()
        .WaitAndRetry(5, (Func<int, TimeSpan>)(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2.0, retryAttempt))),
            (Action<Exception, TimeSpan>)((_, _) => { }));

    public void Dispose()
    {
    }
}
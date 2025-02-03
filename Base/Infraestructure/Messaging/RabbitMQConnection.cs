using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Reflection;
using BaseInfraestructure.Messaging.Interfaces;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace BaseInfraestructure.Messaging;

public sealed class RabbitMQConnection: IRabbitMQConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private readonly int _retryCount;
    private bool _disposed;
    public string ClientProvidedName { get; }
    private readonly object sync_root = new();

    public RabbitMQConnection(IConnectionFactory connectionFactory, string clientProvidedName)
    {
        _retryCount = 5;
        _connectionFactory = connectionFactory;
        ClientProvidedName = GetProviderName(clientProvidedName);
    }

    private string GetProviderName(string clientProvidedName)
    {
        if(string.IsNullOrEmpty(clientProvidedName))
            return Assembly.GetEntryAssembly()?.GetName().Name?.ToUpper() ?? string.Empty;

        return clientProvidedName;
    }

    [MemberNotNullWhen(true, nameof(_connection))]
    public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

    public IModel CreateModel()
    {
        if (!IsConnected)
            throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");

        return _connection.CreateModel();
    }

    public bool TryConnect()
    {
        lock (sync_root)
        {
            Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(
                    _retryCount,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (_, _) => {}
                ).Execute(
                    () => _connection = _connectionFactory.CreateConnection(ClientProvidedName)
                );

            if (!IsConnected)
                return false;

            _connection.ConnectionShutdown += OnConnectionShutdown;
            _connection.CallbackException += OnCallbackException;
            _connection.ConnectionBlocked += OnConnectionBlocked;
            return true;
        }
    }

    public void Dispose()
    {
        if (_disposed)
            return;
        _disposed = true;
        try
        {
            _connection?.Dispose();
        }
        catch (IOException) {}
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        if (_disposed)
            return;
        TryConnect();
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs reason)
    {
        if (_disposed)
            return;
        TryConnect();
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        if (_disposed)
            return;
        TryConnect();
    }
}
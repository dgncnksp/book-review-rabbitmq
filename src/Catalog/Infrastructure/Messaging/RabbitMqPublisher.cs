using System.Text;
using RabbitMQ.Client;

namespace BookReview.Catalog.Infrastructure.Messaging;

public sealed class RabbitMqPublisher : IAsyncDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    private RabbitMqPublisher(
        RabbitMqOptions options,
        IConnection connection,
        IChannel channel)
    {
        _options = options;
        _connection = connection;
        _channel = channel;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(
        RabbitMqOptions options)
    {
        var factory = new ConnectionFactory
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            ClientProvidedName = "book-review-catalog-outbox-publisher",
            AutomaticRecoveryEnabled = true
        };

        var connection = await factory.CreateConnectionAsync();

        var channel = await connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true));

        await channel.ExchangeDeclareAsync(
            options.ExchangeName,
            ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        return new RabbitMqPublisher(
            options,
            connection,
            channel);
    }

    public async Task PublishAsync(
        string type,
        string payload,
        Guid eventId,
        CancellationToken cancellationToken)
    {
        var body = Encoding.UTF8.GetBytes(payload);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
            MessageId = eventId.ToString(),
            Type = type
        };

        await _channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: type,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}

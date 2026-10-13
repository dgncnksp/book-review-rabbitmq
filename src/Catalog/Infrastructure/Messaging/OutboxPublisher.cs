using System.Text.Json;
using BookReview.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookReview.Catalog.Infrastructure.Messaging;

public sealed class OutboxPublisher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly RabbitMqPublisher _rabbitMqPublisher;
    private readonly ILogger<OutboxPublisher> _logger;

    public OutboxPublisher(
        IServiceScopeFactory scopeFactory,
        RabbitMqPublisher rabbitMqPublisher,
        ILogger<OutboxPublisher> logger)
    {
        _scopeFactory = scopeFactory;
        _rabbitMqPublisher = rabbitMqPublisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PublishPendingMessagesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error while publishing outbox messages.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(2),
                stoppingToken);
        }
    }

    private async Task PublishPendingMessagesAsync(
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();

        var dbContext =
            scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        var messages = await dbContext.OutboxMessages
            .Where(x => x.ProcessedOnUtc == null)
            .OrderBy(x => x.OccurredOnUtc)
            .Take(20)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            await _rabbitMqPublisher.PublishAsync(
                message.Type,
                message.Payload,
                message.Id,
                cancellationToken);

            message.MarkAsProcessed(DateTime.UtcNow);

            await dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Published outbox message {MessageId} with type {Type}.",
                message.Id,
                message.Type);
        }
    }
}

using System.Text.Json;
using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Application.IntegrationEvents;

namespace BookReview.Catalog.Infrastructure.Persistence.Outbox;

public sealed class OutboxWriter : IOutboxWriter
{
    private readonly CatalogDbContext _dbContext;

    public OutboxWriter(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(BookCreatedIntegrationEvent integrationEvent)
    {
        var message = new OutboxMessage(
            integrationEvent.EventId,
            integrationEvent.GetType().FullName!,
            JsonSerializer.Serialize(integrationEvent),
            integrationEvent.OccurredOnUtc);

        _dbContext.OutboxMessages.Add(message);
    }
}

using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Application.IntegrationEvents;
using BookReview.Catalog.Domain.Events;

namespace BookReview.Catalog.Application.Events;

public sealed class BookCreatedDomainEventHandler
    : IDomainEventHandler<BookCreatedDomainEvent>
{
    private readonly IOutboxWriter _outboxWriter;

    public BookCreatedDomainEventHandler(
        IOutboxWriter outboxWriter)
    {
        _outboxWriter = outboxWriter;
    }

    public Task Handle(
        BookCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var integrationEvent = new BookCreatedIntegrationEvent(
            Guid.NewGuid(),
            domainEvent.BookId.Value,
            domainEvent.Isbn.Value,
            domainEvent.Title,
            domainEvent.Author,
            DateTime.UtcNow);

        _outboxWriter.Add(integrationEvent);

        return Task.CompletedTask;
    }
}

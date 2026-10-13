using BookReview.Catalog.Application.IntegrationEvents;

namespace BookReview.Catalog.Application.Abstractions;

public interface IOutboxWriter
{
    void Add(BookCreatedIntegrationEvent integrationEvent);
}

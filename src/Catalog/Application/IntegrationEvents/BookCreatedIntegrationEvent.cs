namespace BookReview.Catalog.Application.IntegrationEvents;

public sealed record BookCreatedIntegrationEvent(
    Guid EventId,
    Guid BookId,
    string Isbn,
    string Title,
    string Author,
    DateTime OccurredOnUtc);

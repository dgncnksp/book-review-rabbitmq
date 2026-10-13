using BookReview.Catalog.Domain.Abstractions;
using BookReview.Catalog.Domain.ValueObjects;

namespace BookReview.Catalog.Domain.Events;

public sealed record BookCreatedDomainEvent(
    BookId BookId,
    Isbn Isbn,
    string Title,
    string Author) : IDomainEvent;

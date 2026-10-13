using BookReview.Catalog.Domain.Abstractions;

namespace BookReview.Catalog.Application.Abstractions;

public interface IDomainEventHandler<in TEvent>
    where TEvent : IDomainEvent
{
    Task Handle(
        TEvent domainEvent,
        CancellationToken cancellationToken = default);
}

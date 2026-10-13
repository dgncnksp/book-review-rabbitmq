using BookReview.Catalog.Domain.Abstractions;

namespace BookReview.Catalog.Application.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}

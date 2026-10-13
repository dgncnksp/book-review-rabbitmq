using System.Reflection;
using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Domain.Abstractions;

namespace BookReview.Catalog.Infrastructure.Persistence;

public sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IReadOnlyCollection<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>)
                .MakeGenericType(domainEvent.GetType());

            var handler = _serviceProvider.GetService(handlerType);

            if (handler is null)
                continue;

            var method = handlerType.GetMethod(
                nameof(IDomainEventHandler<IDomainEvent>.Handle));

            if (method is null)
                continue;

            var task = (Task?)method.Invoke(
                handler,
                [domainEvent, cancellationToken]);

            if (task is not null)
                await task;
        }
    }
}

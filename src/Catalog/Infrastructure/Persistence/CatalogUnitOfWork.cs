using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Catalog.Infrastructure.Persistence;

public sealed class CatalogUnitOfWork : IUnitOfWork
{
    private readonly CatalogDbContext _dbContext;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public CatalogUnitOfWork(
        CatalogDbContext dbContext,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _dbContext = dbContext;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var domainEvents = _dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            await _domainEventDispatcher.DispatchAsync(
                domainEvents,
                cancellationToken);

            await _dbContext.SaveChangesAsync(
                cancellationToken);

            await transaction.CommitAsync(
                cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(
                cancellationToken);

            throw;
        }

        foreach (var entry in _dbContext.ChangeTracker
                     .Entries<AggregateRoot>())
        {
            entry.Entity.ClearDomainEvents();
        }
    }
}

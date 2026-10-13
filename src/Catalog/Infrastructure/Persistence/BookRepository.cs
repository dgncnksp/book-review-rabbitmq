using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Domain.Entities;
using BookReview.Catalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Catalog.Infrastructure.Persistence;

public sealed class BookRepository : IBookRepository
{
    private readonly CatalogDbContext _dbContext;

    public BookRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Books.AddAsync(book, cancellationToken);
    }

    public Task<Book?> GetByIdAsync(
        BookId id,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Books
            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}

using BookReview.Catalog.Domain.Entities;
using BookReview.Catalog.Domain.ValueObjects;

namespace BookReview.Catalog.Application.Abstractions;

public interface IBookRepository
{
    Task AddAsync(Book book, CancellationToken cancellationToken = default);

    Task<Book?> GetByIdAsync(
        BookId id,
        CancellationToken cancellationToken = default);
}

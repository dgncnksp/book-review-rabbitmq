using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Application.Commands.CreateBook;
using BookReview.Catalog.Domain.Entities;
using BookReview.Catalog.Domain.ValueObjects;

namespace Catalog.Application.Tests;

public class CreateBookHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateBookAndStoreIt()
    {
        var repository = new FakeBookRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new CreateBookHandler(repository, unitOfWork);

        var command = new CreateBookCommand(
            "9780132350884",
            "Clean Code",
            "Robert C. Martin");

        var result = await handler.Handle(command);

        Assert.NotEqual(default, result.BookId);
        Assert.NotNull(repository.Book);
        Assert.Equal(result.BookId, repository.Book!.Id);
        Assert.Equal("Clean Code", repository.Book.Title);
        Assert.Equal("Robert C. Martin", repository.Book.Author);
    }


    private sealed class FakeUnitOfWork : IUnitOfWork
    {
        public Task SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeBookRepository : IBookRepository
    {
        public Book? Book { get; private set; }

        public Task AddAsync(
            Book book,
            CancellationToken cancellationToken = default)
        {
            Book = book;
            return Task.CompletedTask;
        }

        public Task<Book?> GetByIdAsync(
            BookId id,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(
                Book?.Id == id ? Book : null);
        }
    }
}

using BookReview.Catalog.Domain.Entities;
using BookReview.Catalog.Domain.Enums;
using BookReview.Catalog.Domain.Events;
using BookReview.Catalog.Domain.ValueObjects;

namespace Catalog.Domain.Tests;

public class BookTests
{
    [Fact]
    public void Create_ShouldCreateDraftBookAndRaiseDomainEvent()
    {
        var isbn = Isbn.Create("9780132350884");

        var book = Book.Create(
            isbn,
            "Clean Code",
            "Robert C. Martin");

        Assert.NotEqual(default, book.Id);
        Assert.Equal("Clean Code", book.Title);
        Assert.Equal("Robert C. Martin", book.Author);
        Assert.Equal(PublicationStatus.Draft, book.Status);

        var domainEvent = Assert.Single(book.DomainEvents);

        Assert.IsType<BookCreatedDomainEvent>(domainEvent);
    }

    [Fact]
    public void Create_ShouldRejectEmptyTitle()
    {
        var isbn = Isbn.Create("9780132350884");

        Assert.Throws<ArgumentException>(() =>
            Book.Create(isbn, "", "Robert C. Martin"));
    }

    [Fact]
    public void Publish_ShouldChangeStatusToPublished()
    {
        var book = Book.Create(
            Isbn.Create("9780132350884"),
            "Clean Code",
            "Robert C. Martin");

        book.Publish();

        Assert.Equal(PublicationStatus.Published, book.Status);
    }

    [Fact]
    public void PublishedBook_ShouldNotAllowTitleChange()
    {
        var book = Book.Create(
            Isbn.Create("9780132350884"),
            "Clean Code",
            "Robert C. Martin");

        book.Publish();

        Assert.Throws<InvalidOperationException>(() =>
            book.ChangeTitle("New Title"));
    }
}

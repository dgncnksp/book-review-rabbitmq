using BookReview.Catalog.Domain.Abstractions;
using BookReview.Catalog.Domain.Enums;
using BookReview.Catalog.Domain.Events;
using BookReview.Catalog.Domain.ValueObjects;

namespace BookReview.Catalog.Domain.Entities;

public sealed class Book : AggregateRoot
{
    public BookId Id { get; private set; }
    public Isbn Isbn { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public PublicationStatus Status { get; private set; }

    private Book(
        BookId id,
        Isbn isbn,
        string title,
        string author)
    {
        Id = id;
        Isbn = isbn;
        Title = title;
        Author = author;
        Status = PublicationStatus.Draft;
    }

    public static Book Create(
        Isbn isbn,
        string title,
        string author)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Book title cannot be empty.", nameof(title));

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Book author cannot be empty.", nameof(author));

        var book = new Book(
            BookId.New(),
            isbn,
            title.Trim(),
            author.Trim());

        book.AddDomainEvent(
            new BookCreatedDomainEvent(
                book.Id,
                book.Isbn,
                book.Title,
                book.Author));

        return book;
    }

    public void ChangeTitle(string title)
    {
        if (Status == PublicationStatus.Published)
            throw new InvalidOperationException(
                "A published book cannot change its title.");

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Book title cannot be empty.",
                nameof(title));

        Title = title.Trim();
    }

    public void ChangeAuthor(string author)
    {
        if (Status == PublicationStatus.Published)
            throw new InvalidOperationException(
                "A published book cannot change its author.");

        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException(
                "Book author cannot be empty.",
                nameof(author));

        Author = author.Trim();
    }

    public void Publish()
    {
        if (Status == PublicationStatus.Published)
            throw new InvalidOperationException("Book is already published.");

        Status = PublicationStatus.Published;
    }

    public void Unpublish()
    {
        if (Status == PublicationStatus.Draft)
            throw new InvalidOperationException("Book is already in draft.");

        Status = PublicationStatus.Draft;
    }

}

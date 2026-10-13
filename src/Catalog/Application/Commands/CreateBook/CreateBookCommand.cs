namespace BookReview.Catalog.Application.Commands.CreateBook;

public sealed record CreateBookCommand(
    string Isbn,
    string Title,
    string Author);

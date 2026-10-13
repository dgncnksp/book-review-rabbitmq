using BookReview.Catalog.Domain.ValueObjects;

namespace BookReview.Catalog.Application.Commands.CreateBook;

public sealed record CreateBookResult(BookId BookId);

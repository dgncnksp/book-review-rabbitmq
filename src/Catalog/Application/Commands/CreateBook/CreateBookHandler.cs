using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Domain.Entities;
using BookReview.Catalog.Domain.ValueObjects;

namespace BookReview.Catalog.Application.Commands.CreateBook;

public sealed class CreateBookHandler
{
    private readonly IBookRepository _bookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookHandler(
        IBookRepository bookRepository,
        IUnitOfWork unitOfWork)
    {
        _bookRepository = bookRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateBookResult> Handle(
        CreateBookCommand command,
        CancellationToken cancellationToken = default)
    {
        var isbn = Isbn.Create(command.Isbn);

        var book = Book.Create(
            isbn,
            command.Title,
            command.Author);

        await _bookRepository.AddAsync(
            book,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookResult(book.Id);
    }
}

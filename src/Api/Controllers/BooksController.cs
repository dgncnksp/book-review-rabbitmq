using BookReview.Catalog.Application.Commands.CreateBook;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Api.Controllers;

[ApiController]
[Route("api/books")]
public sealed class BooksController : ControllerBase
{
    private readonly CreateBookHandler _handler;

    public BooksController(CreateBookHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(
            new CreateBookCommand(
                request.Isbn,
                request.Title,
                request.Author),
            cancellationToken);

        return Created(
            $"/api/books/{result.BookId.Value}",
            result);
    }
}

public sealed record CreateBookRequest(
    string Isbn,
    string Title,
    string Author);

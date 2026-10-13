namespace BookReview.Catalog.Domain.ValueObjects;

public readonly record struct BookId(Guid Value)
{
    public static BookId New() => new(Guid.NewGuid());

    public static BookId From(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("BookId cannot be empty.", nameof(value));

        return new BookId(value);
    }

    public override string ToString() => Value.ToString();
}

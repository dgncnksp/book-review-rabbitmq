namespace BookReview.Catalog.Domain.ValueObjects;

public sealed record Isbn
{
    public string Value { get; }

    private Isbn(string value)
    {
        Value = value;
    }

    public static Isbn Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ISBN cannot be empty.", nameof(value));

        var normalized = value.Replace("-", "").Replace(" ", "").Trim();

        if (normalized.Length is not (10 or 13) ||
            normalized.Any(c => !char.IsDigit(c) && c != 'X'))
        {
            throw new ArgumentException("Invalid ISBN format.", nameof(value));
        }

        return new Isbn(normalized);
    }

    public override string ToString() => Value;
}

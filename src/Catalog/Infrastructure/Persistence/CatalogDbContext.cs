using BookReview.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using BookReview.Catalog.Infrastructure.Persistence.Outbox;

namespace BookReview.Catalog.Infrastructure.Persistence;

public sealed class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var book = modelBuilder.Entity<Book>();

        book.ToTable("books");

        book.HasKey(x => x.Id);

        book.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => BookReview.Catalog.Domain.ValueObjects.BookId.From(value))
            .ValueGeneratedNever();

        book.Property(x => x.Isbn)
            .HasConversion(
                isbn => isbn.Value,
                value => BookReview.Catalog.Domain.ValueObjects.Isbn.Create(value))
            .HasMaxLength(13)
            .IsRequired();

        book.Property(x => x.Title)
            .HasMaxLength(300)
            .IsRequired();

        book.Property(x => x.Author)
            .HasMaxLength(200)
            .IsRequired();

        book.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        book.Ignore(x => x.DomainEvents);

        var outbox = modelBuilder.Entity<OutboxMessage>();

        outbox.ToTable("outbox_messages");

        outbox.HasKey(x => x.Id);

        outbox.Property(x => x.Type)
            .HasMaxLength(500)
            .IsRequired();

        outbox.Property(x => x.Payload)
            .IsRequired();

        outbox.Property(x => x.OccurredOnUtc)
            .IsRequired();

        outbox.Property(x => x.ProcessedOnUtc);
    }
}

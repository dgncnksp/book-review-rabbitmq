using BookReview.Catalog.Application.Abstractions;
using BookReview.Catalog.Infrastructure.Persistence;
using BookReview.Catalog.Infrastructure.Persistence.Outbox;
using BookReview.Catalog.Infrastructure.Messaging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookReview.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCatalogInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUnitOfWork, CatalogUnitOfWork>();
        services.AddScoped<IOutboxWriter, OutboxWriter>();

        var rabbitMqOptions = new RabbitMqOptions
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "bookreview",
            Password = "bookreview",
            ExchangeName = "catalog.events"
        };

        services.AddSingleton(rabbitMqOptions);

        services.AddSingleton(sp =>
            RabbitMqPublisher.CreateAsync(rabbitMqOptions)
                .GetAwaiter()
                .GetResult());

        services.AddHostedService<OutboxPublisher>();
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<
            IDomainEventHandler<BookReview.Catalog.Domain.Events.BookCreatedDomainEvent>,
            BookReview.Catalog.Application.Events.BookCreatedDomainEventHandler>();

        return services;
    }
}

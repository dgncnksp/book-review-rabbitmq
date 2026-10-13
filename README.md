# BookReview

BookReview is a .NET 9 application built with a layered architecture and Domain-Driven Design (DDD) principles. It uses PostgreSQL for persistence and RabbitMQ for asynchronous messaging through the Outbox pattern.

## Architecture

The solution consists of API, Catalog, and Review modules.

    BookReview
    ├── src
    │   ├── Api
    │   ├── Catalog
    │   │   ├── Domain
    │   │   ├── Application
    │   │   └── Infrastructure
    │   └── Review
    │       ├── Domain
    │       ├── Application
    │       └── Infrastructure
    └── tests
        ├── Catalog.Domain.Tests
        └── Catalog.Application.Tests

### Layers

- **API:** Exposes HTTP endpoints and delegates requests to application handlers.
- **Domain:** Defines entities, value objects, business rules, and domain events.
- **Application:** Coordinates use cases and application workflows.
- **Infrastructure:** Implements database access, persistence, and messaging.

The Catalog module contains the implemented book creation workflow. The Review module currently provides project structure but does not yet contain implemented review functionality.

## Book Creation Flow

    HTTP Request
         |
         v
    BooksController
         |
         v
    CreateBookHandler
         |
         v
    Book Aggregate
         |
         v
    Repository + Unit of Work
         |
         v
    PostgreSQL

The domain model enforces business rules and raises domain events during book creation.

## Outbox Pattern

Integration events are stored in PostgreSQL and published asynchronously to RabbitMQ by a background service.

    Book Created
         |
         v
    Domain Event
         |
         v
    Integration Event
         |
         v
    Outbox Table
         |
         v
    OutboxPublisher
         |
         v
    RabbitMQ Exchange
    catalog.events

The publisher processes pending outbox messages and marks them as processed after successful publication. Since duplicate delivery is possible, message consumers should be idempotent.

## Technology Stack

- .NET 9
- ASP.NET Core
- C#
- Domain-Driven Design (DDD)
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- xUnit

## Tests

The solution includes domain and application tests covering book creation and core business rules.

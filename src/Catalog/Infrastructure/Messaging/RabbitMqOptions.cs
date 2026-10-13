namespace BookReview.Catalog.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "bookreview";
    public string Password { get; set; } = "bookreview";
    public string ExchangeName { get; set; } = "catalog.events";
}

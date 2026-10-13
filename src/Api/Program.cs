using BookReview.Catalog.Application.Commands.CreateBook;
using BookReview.Catalog.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("Catalog")
    ?? "Host=localhost;Port=5434;Database=bookreview;Username=bookreview;Password=bookreview";

builder.Services.AddCatalogInfrastructure(connectionString);
builder.Services.AddScoped<CreateBookHandler>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using InMemory_Caching.Data;
using InMemory_Caching.Entites;
using InMemory_Caching.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Builder;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext> (options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); // this line is used to configure the database context to use PostgreSQL as the database provider and it retrieves the connection string from the configuration settings using the key "DefaultConnection"
});

builder.Services.AddOpenApi();

builder.Services.AddMemoryCache(o =>
{
    o.SizeLimit = 1024; // this line is used to set the maximum size of the in-memory cache to 1024 units (e.g., bytes, items, etc.) and it helps to prevent the cache from consuming too much memory and allows for better control over cache usage
});


builder.Services.AddScoped<InMemory_Caching.Service.IProductService, InMemory_Caching.Service.ProductService>(); // this line is used to register the ProductService implementation of the IProductService interface with the dependency injection container and it allows for the service to be injected into other components that require it, such as controllers or other services

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => "Hello World!");
var products = app.MapGroup("/products").WithTags("Products");
products.MapGet("/", async (IProductService service, CancellationToken cancellationToken) =>
{
    var result = await service.GetAllAsync(cancellationToken);
    return TypedResults.Ok(result);
})
.WithName("GetAllProducts")
.WithSummary("Get all products")
.WithDescription("Returns all products from the database or cache.");



products.MapGet("/{id:guid}", async (Guid id, IProductService service, CancellationToken cancellationToken) =>
{
    var product = await service.GetByIdAsync(id, cancellationToken);
    return product is not null
        ? TypedResults.Ok(product)
        : Results.NotFound();
})
.WithName("GetProductById")
.WithSummary("Get a product by ID")
.WithDescription("Returns a single product by its ID from the database or cache.");



products.MapPost("/", async (ProductCreationDto request, IProductService service, CancellationToken cancellationToken) =>
{
    var product = await service.CreateAsync(request, cancellationToken);
    return TypedResults.Created($"/products/{product.Id}", product);
})
.WithName("CreateProduct")
.WithSummary("Create a new product")
.WithDescription("Creates a new product and invalidates the product list cache.");

app.Run();

app.Run();

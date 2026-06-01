using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Validation;
using MinimalApis.Filter;
using MinimalApis.Models;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddValidation();
var app = builder.Build();

app.MapScalarApiReference();
app.MapGet("/health", () => "healthy");


var products = new List<Product>
{
      new(1, "Mechanical Keyboard", "Electronics", 149.99m, 50),
    new(2, "Wireless Mouse", "Electronics", 49.99m, 100),
    new(3, "USB-C Hub", "Accessories", 79.99m, 75),
    new(4, "Monitor Stand", "Accessories", 89.99m, 30),
    new(5, "Webcam HD", "Electronics", 129.99m, 45)
};
var nextId = 6;
var productsGroup = app.MapGroup("/api/products").WithTags("Products").AddEndpointFilter<LoggingFilter>();


productsGroup.MapGet("/", Results<Ok<List<Product>>, NoContent> (
string? category,
decimal? minPrice,
decimal? maxPrice,
int page = 1,
int pageSize = 10
) =>
{
    var query = products.AsEnumerable();

    if (!string.IsNullOrEmpty(category))
        query = query.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase));

    if (minPrice.HasValue)
        query = query.Where(p => p.Price >= minPrice);

    var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    return result.Count > 0 ? TypedResults.Ok(result) : TypedResults.NoContent();
}).WithName("GetProducts").WithSummary("Gets all products")
.WithDescription("Returns a paginated list of products with optional filtering by category and price range.");


productsGroup.MapGet("/{id:int}", Results<Ok<Product>, NotFound> (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? TypedResults.Ok(product) : TypedResults.NotFound();
})
.WithName("GetProductById")
.WithSummary("Gets a product by ID")
.WithDescription("Returns a single product based on the provided ID. Returns 404 if not found.");


app.MapGet("/category/{category}", Results<Ok<List<Product>> ,  NotFound> (string category) =>
{
    var result = products
        .Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
        .ToList();
        return result.Count > 0 ? TypedResults.Ok(result) : TypedResults.NotFound();

})
.WithName("GetProductsByCategory")
.WithSummary("Gets products by category")
.WithDescription("Returns all products in the specified category.");


// post


productsGroup.MapPost("/", Results<Created<Product>, BadRequest<string>> (CreateProductRequest request) =>
{
    var product = new Product
    (
        nextId++,
        request.Name,
        request.Category,
        request.Price,
        request.Stock
    );
   products.Add(product);
    return TypedResults.Created($"/api/products/{product.Id}", product);
});



// update 

productsGroup.MapPut("/{id:int}", Results<NoContent, NotFound, BadRequest<string>> (int id, UpdateProductRequest request) =>
{
   var index = products.FindIndex(p => p.Id == id);

   if( index == -1) return TypedResults.NotFound();


   products[index] = new Product (
    id, 
    request.Name,
    request.Category,
    request.Price,
    request.Stock
   );
   return TypedResults.NotFound();
})
.WithName("UpdateProduct")
.WithSummary("Updates an existing product")
.WithDescription("Updates all properties of an existing product. Returns 404 if the product doesn't exist.");


productsGroup.MapDelete("/{id:int}" , Results<NoContent, NotFound> (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);

    if(product is null) return TypedResults.NotFound();

    products.Remove(product);
    return TypedResults.NoContent();
})
.WithName("DeleteProduct")
.WithSummary("Deletes a product")
.WithDescription("Permanently removes a product from the catalog. Returns 404 if the product doesn't exist.");

productsGroup.MapGet("/stats", (ILogger<Program> logger) =>
{
    var stats = new
    {
        TotalProducts = products.Count(),
        TotalStock = products.Sum(p => p.Stock),
        AveragePrice = products.Average(p => p.Price),
         Categories = products.GroupBy(p => p.Category)
            .Select(g => new { Category = g.Key, Count = g.Count() })
            .ToList()
    };
    logger.LogInformation("Statistics requested: {TotalProducts} products", stats.TotalProducts);
    return TypedResults.Ok(stats);
})
.WithName("GetProductStats")
.WithSummary("Gets product statistics")
.WithDescription("Returns aggregate statistics about the product catalog including total count, stock, and category breakdown.");


app.Run();

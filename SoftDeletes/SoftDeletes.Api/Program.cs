using Microsoft.EntityFrameworkCore;
using SoftDeletes.Api.Entites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(sp.GetRequiredService<SoftDeleteInterceptor>());

});

builder.Services.AddScoped<SoftDeleteInterceptor>();

var app = builder.Build();
await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
app.MapGet("/hello", () => "Hello World!")
    .WithName("HelloWorld");


app.MapGet("/categories", async (AppDbContext db, CancellationToken ct) =>
{
    var categories = await db.Categories.Include(c => c.Products).ToListAsync(ct);

    if (categories is null) return Results.NotFound();
    return Results.Ok(categories);
});

app.MapPost("/categories", async (AppDbContext db, CancellationToken ct, string name) =>
{
    var category = new Category { Name = name };
    await db.Categories.AddAsync(category, ct);
    await db.SaveChangesAsync(ct);
    return Results.Created($"/categories/{category.Id}", category);
});

app.MapDelete("/categories/{id:guid}", async (AppDbContext db, CancellationToken ct, Guid id) =>
{
    var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);


    if (category is null) return Results.NotFound();
    db.Categories.Remove(category);
    await db.SaveChangesAsync(ct);
    return Results.NoContent();
});


app.MapPost("/categories/{id:guid}/restore", async (AppDbContext db, CancellationToken ct, Guid id) =>
{
    var category = await db.Categories.IgnoreQueryFilters(["SoftDelete"]).Include(c => c.Products.Where(p => p.IsDeleted)).FirstOrDefaultAsync(c => c.Id == id, ct);
    if (category is null) return Results.NotFound();
    category.IsDeleted = false;

    category.IsDeleted = false;
    category.DeletedAt = null;
    category.DeletedBy = null;

    foreach (var product in category.Products)
    {
        product.IsDeleted = false;
        product.DeletedAt = null;
        product.DeletedBy = null;
    }


    await db.SaveChangesAsync(ct);
    return Results.Ok(category);

});







app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

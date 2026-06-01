var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MovieDbContext> (FileOptions =>
{
    var connString = builder.Configurations.GetConnectionString("DefaultConnection");
    FileOptions.UseNpgsql(connString);
});

builder.Services.AddTransient<IMovieService, MovieService>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();

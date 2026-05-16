using yt_aspcore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


List<GameDto> games = [
    new (
        1,
        "street figher",
        "action",
        19.2M,
        new DateOnly(1992,6,7)
    ),
     new (
        2,
        "street figher2",
        "action2",
        19.2M,
        new DateOnly(1992,6,7)
    ),
     new (
        3,
        "street figher3",
        "action3",
        19.2M,
        new DateOnly(1992,6,7)
    )
];



app.MapGet("/game", () => games);
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id));

app.Run();

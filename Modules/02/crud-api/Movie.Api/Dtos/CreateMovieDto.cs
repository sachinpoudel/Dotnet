namespace Movie.Api.Dtos;


public record CreateMovieDto (string Title, string Genre, DateTimeOffset ReleaseDate, double Rating );
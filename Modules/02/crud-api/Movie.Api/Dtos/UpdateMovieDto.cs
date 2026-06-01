namespace MovieApi.Api.Dtos;

public record UpdateMovieDto(string Title, string Genre, DateTimeOffset ReleaseDate, double Rating);
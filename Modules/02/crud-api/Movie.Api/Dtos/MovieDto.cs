
namespace Movie.Api.Dtos;
public record MovieDto(Guid Id, string Title , string Genre, DateTimeOffset ReleaseDate, DateTimeOffset Rating);
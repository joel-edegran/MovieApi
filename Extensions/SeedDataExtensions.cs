using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Extensions;

public class ActorSeedDto
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class MovieSeedDto
{
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int Duration { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? Synopsis {  get; set; }
    public string? Language {  get; set; }
    public decimal? Budget {  get; set; }
    public List<ActorSeedDto> Actors { get; set; } = new();
}

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

        if (await context.Movies.AnyAsync())
        {
            return;
        }

        var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "movies.json");
        if (!File.Exists(jsonFilePath))
        {
            return;
        }

        var jsonString = await File.ReadAllTextAsync(jsonFilePath);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var movieDtos = JsonSerializer.Deserialize<List<MovieSeedDto>>(jsonString, options);

        if (movieDtos == null || !movieDtos.Any())
        {
            return;
        }

        var genreMap = movieDtos
            .Where(m => !string.IsNullOrWhiteSpace(m.Genre))
            .Select(m => m.Genre.Trim())
            .Distinct()
            .ToDictionary(name => name, name => new Genre { Name = name });

        var directorMap = movieDtos
            .Where(m => !string.IsNullOrWhiteSpace(m.Director))
            .Select(m => m.Director.Trim())
            .Distinct()
            .ToDictionary(name => name, name => new Director { Name = name });

        var countryMap = movieDtos
            .Where(m => !string.IsNullOrWhiteSpace(m.Country))
            .Select(m => m.Country.Trim())
            .Distinct()
            .ToDictionary(name => name, name => new Country { Name = name });

        var actorMap = movieDtos
            .SelectMany(m => m.Actors)
            .Where(a => !string.IsNullOrWhiteSpace(a.Name))
            .Select(a => a.Name.Trim())
            .Distinct()
            .ToDictionary(name => name, name => new Actor { Name = name });

        await context.Genres.AddRangeAsync(genreMap.Values);
        await context.Directors.AddRangeAsync(directorMap.Values);
        await context.Countries.AddRangeAsync(countryMap.Values);
        await context.Actors.AddRangeAsync(actorMap.Values);

        var movies = new List<Movie>();
        var movieActors = new List<MovieActor>();

        foreach (var dto in movieDtos)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                ReleaseYear = dto.ReleaseYear,
                Duration = dto.Duration,
                Genre = !string.IsNullOrWhiteSpace(dto.Genre) && genreMap.TryGetValue(dto.Genre.Trim(), out var g) ? g : null,
                Director = !string.IsNullOrWhiteSpace(dto.Director) && directorMap.TryGetValue(dto.Director.Trim(), out var d) ? d : null,
                Country = !string.IsNullOrWhiteSpace(dto.Country) && countryMap.TryGetValue(dto.Country.Trim(), out var c) ? c : null,
                Details = !string.IsNullOrWhiteSpace(dto.Language) || !string.IsNullOrWhiteSpace(dto.Synopsis) || dto.Budget.HasValue ? new MovieDetails
                {
                    Synopsis = dto.Synopsis,
                    Language = dto.Language,
                    Budget = dto.Budget
                } : null
            };

            movies.Add(movie);

            foreach (var actorDto in dto.Actors)
            {
                if (!string.IsNullOrWhiteSpace(actorDto.Name) && actorMap.TryGetValue(actorDto.Name.Trim(), out var actor))
                {
                    movieActors.Add(new MovieActor
                    {
                        Movie = movie,
                        Actor = actor,
                        Role = actorDto.Role.Trim()
                    });
                }
            }
        }

        await context.Movies.AddRangeAsync(movies);
        await context.MovieActors.AddRangeAsync(movieActors);

        await context.SaveChangesAsync();
    }
}

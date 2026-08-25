using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Extensions;

public static class SeedDataExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

        if (await context.Movies.AnyAsync() || await context.Actors.AnyAsync())
        {
            return;
        }

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data");

        // Actors
        var actorsFilePath = Path.Combine(dataDirectory, "actors.json");
        List<Actor> actors = new();
        var actorEntityMap = new Dictionary<int, Actor>();

        if (File.Exists(actorsFilePath))
        {
            var actorJsonString = await File.ReadAllTextAsync(actorsFilePath);
            if (!actorJsonString.TrimStart().StartsWith("["))
            {
                actorJsonString = "[\n" + actorJsonString + "\n]";
            }

            var actorDtos = JsonSerializer.Deserialize<List<ActorJsonDto>>(actorJsonString, options);
            if (actorDtos != null)
            {
                foreach (var dto in actorDtos)
                {
                    var actor = new Actor
                    {
                        Name = dto.Name,
                        BirthYear = dto.BirthYear
                    };
                    actors.Add(actor);
                }

                await context.Actors.AddRangeAsync(actors);
                await context.SaveChangesAsync();

                for (int i = 0; i < actorDtos.Count; i++)
                {
                    actorEntityMap[actorDtos[i].ActorId] = actors[i];
                }
            }
        }

        // Movies
        var moviesFilePath = Path.Combine(dataDirectory, "movies.json");
        if (!File.Exists(moviesFilePath))
        {
            return;
        }

        var movieJsonString = await File.ReadAllTextAsync(moviesFilePath);
        if (!movieJsonString.TrimStart().StartsWith("["))
        {
            movieJsonString = "[\n" + movieJsonString + "\n]";
        }

        var movieDtos = JsonSerializer.Deserialize<List<MovieSeedDto>>(movieJsonString, options);
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

        await context.Genres.AddRangeAsync(genreMap.Values);
        await context.Directors.AddRangeAsync(directorMap.Values);
        await context.Countries.AddRangeAsync(countryMap.Values);
        await context.SaveChangesAsync();

        var movies = new List<Movie>();
        var movieActors = new List<MovieActor>();

        // Track already processed movies in this batch to avoid internal duplicates
        var processedMovieKeys = new HashSet<string>();

        foreach (var dto in movieDtos)
        {
            var directorName = !string.IsNullOrWhiteSpace(dto.Director) ? dto.Director.Trim() : string.Empty;
            var uniqueKey = $"{dto.Title?.Trim().ToLower()}_{dto.ReleaseYear}_{directorName.ToLower()}";

            if (processedMovieKeys.Contains(uniqueKey))
            {
                continue; // Skip duplicate within the JSON file
            }

            // Check if the movie already exists in the database by Title, ReleaseYear, and Director
            var movieExistsInDb = await context.Movies
                .Include(m => m.Director)
                .AnyAsync(m => m.Title.ToLower() == dto.Title.ToLower() &&
                               m.ReleaseYear == dto.ReleaseYear &&
                               ((m.Director == null && string.IsNullOrEmpty(directorName)) ||
                                (m.Director != null && m.Director.Name.ToLower() == directorName.ToLower())));

            if (movieExistsInDb)
            {
                continue; // Skip if it already exists in the database
            }

            processedMovieKeys.Add(uniqueKey);

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
            await context.Movies.AddAsync(movie);
            await context.SaveChangesAsync();

            foreach (var actorRef in dto.Actors)
            {
                if (actorEntityMap.TryGetValue(actorRef.ActorId, out var actor))
                {
                    movieActors.Add(new MovieActor
                    {
                        MovieId = movie.Id,
                        ActorId = actor.Id,
                        Role = actorRef.Role.Trim()
                    });
                }
            }
        }

        await context.MovieActors.AddRangeAsync(movieActors);
        await context.SaveChangesAsync();

        // Reviews
        var reviewsFilePath = Path.Combine(dataDirectory, "reviews.json");
        if (File.Exists(reviewsFilePath))
        {
            var reviewJsonString = await File.ReadAllTextAsync(reviewsFilePath);
            if (!reviewJsonString.TrimStart().StartsWith("["))
            {
                reviewJsonString = "[\n" + reviewJsonString + "\n]";
            }

            var reviewDtos = JsonSerializer.Deserialize<List<ReviewSeedDto>>(reviewJsonString, options);
            if (reviewDtos != null && reviewDtos.Any())
            {
                var reviews = new List<Review>();
                foreach (var dto in reviewDtos)
                {
                    var movieExists = await context.Movies.AnyAsync(m => m.Id == dto.MovieId);
                    if (movieExists)
                    {
                        reviews.Add(new Review
                        {
                            MovieId = dto.MovieId,
                            ReviewerName = dto.ReviewerName,
                            Comment = dto.Comment,
                            Rating = dto.Rating
                        });
                    }
                }

                if (reviews.Any())
                {
                    await context.Reviews.AddRangeAsync(reviews);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

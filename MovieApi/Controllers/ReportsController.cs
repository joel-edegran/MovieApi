using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;

namespace MovieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly MovieContext _context;

    public ReportsController(MovieContext context)
    {
        _context = context;
    }

    [HttpGet("movies/top5pergenre")]
    public async Task<IActionResult> GetTop5PerGenre()
    {
        var result = await _context.Genres
            .Select(genre => new
            {
                Genre = new
                {
                    genre.Id,
                    genre.Name,
                    Movies = genre.Movies
                        .Where(m => m.Reviews.Any())
                        .OrderByDescending(m => m.Reviews.Average(r => r.Rating))
                        .Take(5)
                        .Select(m => new
                        {
                            m.Id,
                            m.Title,
                            AverageRating = m.Reviews.Average(r => r.Rating)
                        })
                        .ToList()
                }
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("movies/average-ratings")]
    public async Task<IActionResult> GetAverageRatingsPerGenre()
    {
        var result = await _context.Movies
            .Where(m => m.Reviews.Any())
            .GroupBy(m => m.Genre)
            .Select(g => new
            {
                Genre = g.Key,
                AverageRating = Math.Round(g.SelectMany(m => m.Reviews).Average(r => r.Rating), 2)
            })
            .OrderByDescending(g => g.AverageRating)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("actors/most-active")]
    public async Task<IActionResult> getMostActiveActors()
    {
        var result = await _context.Actors
            .Select(a => new
            {
                a.Id,
                a.Name,
                MovieCount = a.MovieActors.Count
            })
            .OrderByDescending(a => a.MovieCount)
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("movies/longest-per-country")]
    public async Task<IActionResult> GetLongestPerCountry()
    {
        var result = await _context.Movies
            .Where(m => m.Details != null && !string.IsNullOrEmpty(m.Details.Language))
            .GroupBy(m => m.Details!.Language)
            .Select(g => new
            {
                LanguageOrCountry = g.Key,
                LongestMovie = g.OrderByDescending(m => m.Duration)
                .Select(m => new { m.Id, m.Title, m.Duration })
                .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("movies/with-most-reviews")]
    public async Task<IActionResult> GetMovieWithMostReviews()
    {
        var result = await _context.Movies
            .Where(m => m.Reviews.Any())
            .Select(m => new
            {
                m.Id,
                m.Title,
                ReviewCount = m.Reviews.Count
            })
            .OrderByDescending(m => m.ReviewCount)
            .FirstOrDefaultAsync();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("genres/popular")]
    public async Task<IActionResult> GetPopularGenres()
    {
        var result = await _context.Genres
            .Select(genre => new
            {
                Genre = new
                {
                    genre.Id,
                    genre.Name,
                    Movies = genre.Movies
                        .Select(m => new
                        {
                            m.Id,
                            m.Title,
                            m.ReleaseYear,
                            m.Duration
                        })
                        .ToList()
                },
                MovieCount = genre.Movies.Count
            })
            .OrderByDescending(g => g.MovieCount)
            .ToListAsync(); 
        
        return Ok(result);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;
    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie()
    {
        return await _context.Movies
            .Select(movie => new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Genre = movie.Genre,
                Year = movie.Year,
                Duration = movie.Duration
            })
            .ToListAsync();
    }

    // GET: api/Movie/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            Year = movie.Year,
            Duration = movie.Duration
        };
    }

    // GET: api/movies/5/details
    [HttpGet("{id}/details")]
    public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
    {
        var movieDetailDto = await _context.Movies
            .Include(movie => movie.Details)
            .Include(movie => movie.Reviews)
            .Include(movie => movie.Actors)
            .Where(movie => movie.Id == id)
            .Select(movie => new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Year = movie.Year,
                Genre = movie.Genre,
                Duration = movie.Duration,

                Synopsis = movie.Details != null ? movie.Details.Synopsis : string.Empty,
                Language = movie.Details != null ? movie.Details.Language : string.Empty,
                Budget = movie.Details != null ? movie.Details.Budget : 0,

                Reviews = movie.Reviews.Select(review => new ReviewDto
                {
                    Id = review.Id,
                    ReviewerName = review.ReviewerName,
                    Comment = review.Comment,
                    Rating = review.Rating
                }).ToList(),

                Actors = movie.Actors.Select(actor => new ActorDto
                {
                    Id = actor.Id,
                    Name = actor.Name,
                    BirthYear = actor.BirthYear
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (movieDetailDto == null)
        {
            return NotFound();
        }

        return Ok(movieDetailDto);
    }

    // PUT: api/Movie/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutMovie(int? id, MovieDto movieDto)
    {
        if (id != movieDto.Id)
        {
            return BadRequest();
        }

        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        movie.Title = movieDto.Title;
        movie.Genre = movieDto.Genre;
        movie.Year = movieDto.Year;
        movie.Duration = movieDto.Duration;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!MovieExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieDto>> PostMovie(MovieDto movieDto)
    {
        var movie = new Movie
        {
            Title = movieDto.Title,
            Genre = movieDto.Genre,
            Year = movieDto.Year,
            Duration = movieDto.Duration
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        movieDto.Id = movie.Id;

        return CreatedAtAction("GetMovie", new { id = movieDto.Id }, movieDto);
    }

    // DELETE: api/Movie/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MovieExists(int? id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
}

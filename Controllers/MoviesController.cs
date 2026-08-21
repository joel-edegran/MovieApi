using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieContext _context;

    public MoviesController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/Movie
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovie(
        [FromQuery] string? genre,
        [FromQuery] int? year,
        [FromQuery] string? actor)
    {
        var query = _context.Movies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(m => m.Genre.Contains(genre));
        }

        if (year.HasValue)
        {
            query = query.AsQueryable().Where(m => m.Year == year.Value);
        }

        if (!string.IsNullOrWhiteSpace(actor)) 
        {
            query = query.Where(m => m.MovieActors.Any(ma => ma.Actor != null && ma.Actor.Name.Contains(actor)));
        }

        return await query
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
            .Include(movie => movie.MovieActors)
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

                Actors = movie.MovieActors.Select(ma => new MovieActorRoleDto
                {
                    Id = ma.Actor!.Id,
                    Name = ma.Actor.Name,
                    BirthYear = ma.Actor.BirthYear,
                    RoleName = ma.Role
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
    public async Task<IActionResult> PutMovie(int id, MovieUpdateDto dto)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie == null)
        {
            return NotFound();
        }

        movie.Title = dto.Title;
        movie.Genre = dto.Genre;
        movie.Year = dto.Year;
        movie.Duration = dto.Duration;

        await _context.SaveChangesAsync();
       
        return NoContent();
    }

    // POST: api/Movie
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MovieDto>> PostMovie(MovieCreateDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Genre = dto.Genre,
            Year = dto.Year,
            Duration = dto.Duration
        };

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            Year = movie.Year,
            Duration = movie.Duration
        };

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieDto);
    }

    // POST: api/movies/5/actors
    [HttpPost("{movieId}/actors")]
    public async Task<IActionResult> AddActorToMovie(int movieId, [FromBody] MovieActorCreateDto dto)
    {
        var movie = await _context.Movies.FindAsync(movieId);
        var actor = await _context.Actors.FindAsync(dto.ActorId);

        if (movie == null || actor == null)
        {
            return NotFound("Movie or Actor not found.");
        }

        var movieActorExists = await _context.MovieActors
            .AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == dto.ActorId);

        if (movieActorExists)
        {
            return BadRequest("Actor is already assigned to this movie.");
        }

        var movieActor = new MovieActor
        {
            MovieId = movieId,
            ActorId = dto.ActorId,
            Role = dto.Role
        };

        _context.MovieActors.Add(movieActor);
        await _context.SaveChangesAsync();

        return Ok();
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

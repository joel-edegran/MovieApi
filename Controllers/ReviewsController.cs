using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly MovieContext _context;

    public ReviewsController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/movies/{movieId}/reviews
    [HttpGet("movies/{movieId}/reviews")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetMovieReviews(int movieId)
    {
        var movieExists = await _context.Movies.AnyAsync(m => m.Id == movieId);
        if (!movieExists)
        {
            return NotFound("Movie not found.");
        }

        var reviews = await _context.Reviews
            .Where(review => review.MovieId == movieId)
            .Select(review => new ReviewDto
            {
                Id = review.Id,
                ReviewerName = review.ReviewerName,
                Comment = review.Comment,
                Rating = review.Rating
            })
            .ToListAsync();

        return Ok(reviews);
    }

    // POST: api/movies/{movieId}/reviews
    [HttpPost("movies/{movieId}/reviews")]
    public async Task<ActionResult<ReviewDto>> CreateReview(int movieId, [FromBody] ReviewCreateDto reviewCreateDto)
    {
        var movieExists = await _context.Movies.AnyAsync(m => m.Id == movieId);
        if (!movieExists)
        {
            return NotFound("Movie not found");
        }

        var review = new Review
        {
            MovieId = movieId,
            ReviewerName = reviewCreateDto.ReviewerName,
            Comment = reviewCreateDto.Comment,
            Rating = reviewCreateDto.Rating
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var reviewDto = new ReviewDto
        {
            Id = review.Id,
            ReviewerName = review.ReviewerName,
            Comment = review.Comment,
            Rating = review.Rating
        };

        return CreatedAtAction(nameof(GetMovieReviews), new { movieId = movieId }, reviewDto);
    }

    // DELETE: api/reviews/{id}
    [HttpDelete("reviews/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        if (review == null)
        {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

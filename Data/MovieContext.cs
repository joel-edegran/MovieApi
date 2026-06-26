using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<MovieDetails> MovieDetails { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Actor> Actors { get; set; } = null!;
}

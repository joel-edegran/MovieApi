using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Data;

public class MovieContext : DbContext
{
    public MovieContext(DbContextOptions<MovieContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Director> Directors => Set<Director>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<MovieDetails> MovieDetails => Set<MovieDetails>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Actor> Actors => Set<Actor>();
    public DbSet<MovieActor> MovieActors => Set<MovieActor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Movie relationships
        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Director)
            .WithMany(d => d.Movies)
            .HasForeignKey(m => m.DirectorId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Genre)
            .WithMany(g => g.Movies)
            .HasForeignKey(m => m.GenreId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Movie>()
            .HasOne(m => m.Country)
            .WithMany(c => c.Movies)
            .HasForeignKey(m => m.CountryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Reviews)
            .WithOne(r => r.Movie)
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        // Many-to-many relationship between Movie and Actor
        modelBuilder.Entity<MovieActor>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        modelBuilder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(a => a.MovieActors)
            .HasForeignKey(ma => ma.ActorId);

        // Properties and precision
        modelBuilder.Entity<MovieDetails>()
            .Property(d => d.Budget)
            .HasPrecision(18, 2);
    }
}

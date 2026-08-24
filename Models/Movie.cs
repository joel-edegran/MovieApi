using System.Collections.Generic;

namespace MovieApi.Models;

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int ReleaseYear { get; set; }
    public int Duration { get; set; }

    public Genre? Genre { get; set; }
    public Director? Director { get; set; }
    public Country? Country { get; set; }

    public int? GenreId { get; set; }
    public int? DirectorId { get; set; }
    public int? CountryId { get; set; }

    public MovieDetails? Details { get; set; }

    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}

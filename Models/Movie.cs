namespace MovieApi.Models;

public class Movie
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public int Duration { get; set; }
    
    public MovieDetails? Details { get; set; }
}

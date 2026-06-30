namespace MovieApi.DTOs;

public class MovieDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public int Year { get; set; }
    public int Duration { get; set; }
}

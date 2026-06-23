namespace MovieApi.Models;

public class MovieDetails
{
    public int Id { get; set; }
    public string Synopsis { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}

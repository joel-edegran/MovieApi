using System.ComponentModel.DataAnnotations.Schema;

namespace MovieApi.Models;

public class MovieDetails
{
    public int Id { get; set; }
    public string? Synopsis { get; set; }
    public required string Language { get; set; }
    public decimal? Budget { get; set; }

    public int MovieId { get; set; }
    
    [ForeignKey("MovieId")]
    public Movie? Movie { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models;

public class Review
{
    public int Id { get; set; }
    public required string Reviewname { get; set; }
    public string? Comment { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public int MovieId { get; set; }
    public Movie? Movie { get; set; }
}

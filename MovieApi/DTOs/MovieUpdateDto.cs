using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class MovieUpdateDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Genre { get; set; }

    [Range(1888, 2100)]
    public int ReleaseYear { get; set; }

    [Range(1, 1000)]
    public int Duration { get; set; }
}

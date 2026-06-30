using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class MovieUpdateDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Genre { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public int Duration { get; set; }
}

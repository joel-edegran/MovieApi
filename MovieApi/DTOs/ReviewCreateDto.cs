using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class ReviewCreateDto
{
    [Required]
    public string ReviewerName { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; }
}
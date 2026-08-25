using System.Collections.Generic;

namespace MovieApi.DTOs;

public class MovieSeedDto
{
    public required string Title { get; set; }
    public int ReleaseYear { get; set; }
    public int Duration { get; set; }
    public required string Genre { get; set; }
    public required string Director { get; set; }
    public required string Country { get; set; }
    public string? Synopsis { get; set; }
    public string? Language { get; set; }
    public decimal? Budget { get; set; }
    public List<MovieActorJsonDto> Actors { get; set; } = new();
}
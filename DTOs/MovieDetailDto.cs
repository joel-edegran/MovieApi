using System.Collections.Generic;

namespace MovieApi.DTOs;

public class MovieDetailDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public int ReleaseYear { get; set; }
    public int Duration { get; set; }

    public string? Synopsis { get; set; }
    public required string Language { get; set; }
    public decimal? Budget { get; set; }

    public List<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    public List<MovieActorRoleDto> Actors { get; set; } = new();
}

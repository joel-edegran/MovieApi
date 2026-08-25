namespace MovieApi.DTOs;

public class MoviePatchDto
{
    public string? Title { get; set; }
    public string? Genre { get; set; }
    public int? ReleaseYear { get; set; }
    public int? Duration { get; set; }
}

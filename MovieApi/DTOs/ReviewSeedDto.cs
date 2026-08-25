namespace MovieApi.DTOs;

public class ReviewSeedDto
{
    public int MovieId { get; set; }
    public required string ReviewerName { get; set; }
    public string? Comment { get; set; }
    public int Rating { get; set; }
}
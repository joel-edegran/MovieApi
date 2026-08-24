namespace MovieApi.DTOs;

public class ActorJsonDto
{
    public int ActorId { get; set; }
    public required string Name { get; set; }
    public int BirthYear { get; set; }
}
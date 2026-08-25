using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs;

public class MovieActorCreateDto
{
    [Required]
    public int ActorId { get; set; }
    [Required] 
    public string Role { get; set;} = string.Empty;
}

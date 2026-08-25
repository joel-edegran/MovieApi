namespace MovieApi.DTOs;

public class MovieActorRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
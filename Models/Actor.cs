using System.Collections;
using System.Collections.Generic;

namespace MovieApi.Models;

public class Actor
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int BirthYear { get; set; }

    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
}

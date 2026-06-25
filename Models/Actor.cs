using System.Collections;
using System.Collections.Generic;

namespace MovieApi.Models;

public class Actor
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int BirthYear { get; set; }

    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}

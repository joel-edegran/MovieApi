using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieApi.Data;
using MovieApi.Models;

namespace MovieApi.Extensions;

public static class SeedDataExtensions
{
    public static async Task SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

        await context.Database.MigrateAsync();

        if (await context.Movies.AnyAsync())
        {
            return;
        }

        var movies = new List<Movie>
        {
            new() { Title = "The Shawshank Redemption",                             Genre = "Drama",        Year = 1994, Duration = 142 },
            new() { Title = "The Godfather",                                        Genre = "Crime",        Year = 1972, Duration = 175 },
            new() { Title = "The Dark Knight",                                      Genre = "Action",       Year = 2008, Duration = 152 },
            new() { Title = "The Godfather Part II",                                Genre = "Crime",        Year = 1974, Duration = 202 },
            new() { Title = "12 Angry Men",                                         Genre = "Crime",        Year = 1957, Duration = 96  },
            new() { Title = "Schindler's List",                                     Genre = "Biography",    Year = 1993, Duration = 195 },
            new() { Title = "The Lord of the Rings: The Return of the King",        Genre = "Action",       Year = 2003, Duration = 201 },
            new() { Title = "Pulp Fiction",                                         Genre = "Crime",        Year = 1994, Duration = 154 },
            new() { Title = "The Lord of the Rings: The Fellowship of the Ring",    Genre = "Action",       Year = 2001, Duration = 178 },
            new() { Title = "The Good, the Bad and the Ugly",                       Genre = "Western",      Year = 1966, Duration = 161 },
            new() { Title = "Forrest Gump",                                         Genre = "Drama",        Year = 1994, Duration = 142 },
            new() { Title = "The Lord of the Rings: The Two Towers",                Genre = "Action",       Year = 2002, Duration = 179 },
            new() { Title = "Fight Club",                                           Genre = "Drama",        Year = 1999, Duration = 139 },
            new() { Title = "Inception",                                            Genre = "Action",       Year = 2010, Duration = 148 },
            new() { Title = "Star Wars: Episode V - The Empire Strikes Back",       Genre = "Action",       Year = 1980, Duration = 124 },
            new() { Title = "The Matrix",                                           Genre = "Action",       Year = 1999, Duration = 136 },
            new() { Title = "Goodfellas",                                           Genre = "Biography",    Year = 1990, Duration = 145 },
            new() { Title = "One Flew Over the Cuckoo's Nest",                      Genre = "Drama",        Year = 1975, Duration = 133 },
            new() { Title = "Se7en",                                                Genre = "Crime",        Year = 1995, Duration = 127 },
            new() { Title = "Seven Samurai",                                        Genre = "Action",       Year = 1954, Duration = 207 },
            new() { Title = "It's a Wonderful Life",                                Genre = "Drama",        Year = 1946, Duration = 130 },
            new() { Title = "The Silence of the Lambs",                             Genre = "Crime",        Year = 1991, Duration = 118 },
            new() { Title = "Saving Private Ryan",                                  Genre = "Drama",        Year = 1998, Duration = 169 },
            new() { Title = "City of God",                                          Genre = "Crime",        Year = 2002, Duration = 130 },
            new() { Title = "Life Is Beautiful",                                    Genre = "Comedy",       Year = 1997, Duration = 116 },
            new() { Title = "The Green Mile",                                       Genre = "Crime",        Year = 1999, Duration = 189 },
            new() { Title = "Interstellar",                                         Genre = "Adventure",    Year = 2014, Duration = 169 },
            new() { Title = "Star Wars: Episode IV - A New Hope",                   Genre = "Action",       Year = 1977, Duration = 121 },
            new() { Title = "Terminator 2: Judgment Day",                           Genre = "Action",       Year = 1991, Duration = 137 },
            new() { Title = "Back to the Future",                                   Genre = "Adventure",    Year = 1985, Duration = 116 },
            new() { Title = "Spirited Away",                                        Genre = "Animation",    Year = 2001, Duration = 125 },
            new() { Title = "The Pianist",                                          Genre = "Biography",    Year = 2002, Duration = 150 },
            new() { Title = "Psycho",                                               Genre = "Horror",       Year = 1960, Duration = 109 },
            new() { Title = "Parasite",                                             Genre = "Drama",        Year = 2019, Duration = 132 },
            new() { Title = "Leon: The Professional",                               Genre = "Action",       Year = 1994, Duration = 110 },
            new() { Title = "The Lion King",                                        Genre = "Animation",    Year = 1994, Duration = 88  },
            new() { Title = "Gladiator",                                            Genre = "Action",       Year = 2000, Duration = 155 },
            new() { Title = "American History X",                                   Genre = "Drama",        Year = 1998, Duration = 119 },
            new() { Title = "The Departed",                                         Genre = "Crime",        Year = 2006, Duration = 151 },
            new() { Title = "Whiplash",                                             Genre = "Drama",        Year = 2014, Duration = 106 },
            new() { Title = "The Prestige",                                         Genre = "Drama",        Year = 2006, Duration = 130 },
            new() { Title = "The Usual Suspects",                                   Genre = "Crime",        Year = 1995, Duration = 106 },
            new() { Title = "Casablanca",                                           Genre = "Drama",        Year = 1942, Duration = 102 },
            new() { Title = "Grave of the Fireflies",                               Genre = "Animation",    Year = 1988, Duration = 89  },
            new() { Title = "Harakiri",                                             Genre = "Action",       Year = 1962, Duration = 133 },
            new() { Title = "Intouchables",                                         Genre = "Biography",    Year = 2011, Duration = 112 },
            new() { Title = "Modern Times",                                         Genre = "Comedy",       Year = 1936, Duration = 87  },
            new() { Title = "Once Upon a Time in the West",                         Genre = "Western",      Year = 1968, Duration = 165 },
            new() { Title = "Cinema Paradiso",                                      Genre = "Drama",        Year = 1988, Duration = 155 },
            new() { Title = "Rear Window",                                          Genre = "Mystery",      Year = 1954, Duration = 112 },
            new() { Title = "Alien",                                                Genre = "Horror",       Year = 1979, Duration = 117 },
            new() { Title = "City Lights",                                          Genre = "Comedy",       Year = 1931, Duration = 87  },
            new() { Title = "Apocalypse Now",                                       Genre = "Drama",        Year = 1979, Duration = 147 },
            new() { Title = "Memento",                                              Genre = "Mystery",      Year = 2000, Duration = 113 },
            new() { Title = "Django Unchained",                                     Genre = "Western",      Year = 2012, Duration = 165 },
            new() { Title = "Raiders of the Lost Ark",                              Genre = "Action",       Year = 1981, Duration = 115 },
            new() { Title = "WALL-E",                                               Genre = "Animation",    Year = 2008, Duration = 98  },
            new() { Title = "The Lives of Others",                                  Genre = "Drama",        Year = 2006, Duration = 137 },
            new() { Title = "Sunset Blvd.",                                         Genre = "Drama",        Year = 1950, Duration = 110 },
            new() { Title = "Paths of Glory",                                       Genre = "War",          Year = 1957, Duration = 88  },
            new() { Title = "The Shining",                                          Genre = "Horror",       Year = 1980, Duration = 146 },
            new() { Title = "The Great Dictator",                                   Genre = "Comedy",       Year = 1940, Duration = 125 },
            new() { Title = "Witness for the Prosecution",                          Genre = "Crime",        Year = 1957, Duration = 116 },
            new() { Title = "Avengers: Infinity War",                               Genre = "Action",       Year = 2018, Duration = 149 },
            new() { Title = "Aliens",                                               Genre = "Action",       Year = 1986, Duration = 137 },
            new() { Title = "American Beauty",                                      Genre = "Drama",        Year = 1999, Duration = 122 },
            new() { Title = "Dr. Strangelove",                                      Genre = "Comedy",       Year = 1964, Duration = 95  },
            new() { Title = "Spider-Man: Into the Spider-Verse",                    Genre = "Animation",    Year = 2018, Duration = 117 },
            new() { Title = "The Dark Knight Rises",                                Genre = "Action",       Year = 2012, Duration = 164 },
            new() { Title = "Oldboy",                                               Genre = "Action",       Year = 2003, Duration = 120 },
            new() { Title = "Joker",                                                Genre = "Crime",        Year = 2019, Duration = 122 },
            new() { Title = "Amadeus",                                              Genre = "Biography",    Year = 1984, Duration = 160 },
            new() { Title = "Toy Story",                                            Genre = "Animation",    Year = 1995, Duration = 81  },
            new() { Title = "Coco",                                                 Genre = "Animation",    Year = 2017, Duration = 105 },
            new() { Title = "Braveheart",                                           Genre = "Biography",    Year = 1995, Duration = 178 },
            new() { Title = "The Boat",                                             Genre = "Drama",        Year = 1981, Duration = 149 },
            new() { Title = "Inglourious Basterds",                                 Genre = "Action",       Year = 2009, Duration = 153 },
            new() { Title = "Princess Mononoke",                                    Genre = "Animation",    Year = 1997, Duration = 134 },
            new() { Title = "Good Will Hunting",                                    Genre = "Drama",        Year = 1997, Duration = 126 },
            new() { Title = "Requiem for a Dream",                                  Genre = "Drama",        Year = 2000, Duration = 102 },
            new() { Title = "Get Out",                                              Genre = "Horror",       Year = 2017, Duration = 104 },
            new() { Title = "Toy Story 3",                                          Genre = "Animation",    Year = 2010, Duration = 103 },
            new() { Title = "Your Name.",                                           Genre = "Animation",    Year = 2016, Duration = 106 },
            new() { Title = "Singin' in the Rain",                                  Genre = "Comedy",       Year = 1952, Duration = 103 },
            new() { Title = "3 Idiots",                                             Genre = "Comedy",       Year = 2009, Duration = 170 },
            new() { Title = "Star Wars: Episode VI - Return of the Jedi",           Genre = "Action",       Year = 1983, Duration = 131 },
            new() { Title = "2001: A Space Odyssey",                                Genre = "Adventure",    Year = 1968, Duration = 149 },
            new() { Title = "Eternal Sunshine of the Spotless Mind",                Genre = "Drama",        Year = 2004, Duration = 108 },
            new() { Title = "High and Low",                                         Genre = "Crime",        Year = 1963, Duration = 143 },
            new() { Title = "Citizen Kane",                                         Genre = "Drama",        Year = 1941, Duration = 119 },
            new() { Title = "Lawrence of Arabia",                                   Genre = "Adventure",    Year = 1962, Duration = 218 },
            new() { Title = "Capernaum",                                            Genre = "Drama",        Year = 2018, Duration = 126 },
            new() { Title = "North by Northwest",                                   Genre = "Adventure",    Year = 1959, Duration = 136 },
            new() { Title = "The Hunt",                                             Genre = "Drama",        Year = 2012, Duration = 115 },
            new() { Title = "Vertigo",                                              Genre = "Mystery",      Year = 1958, Duration = 128 },
            new() { Title = "Amelie",                                               Genre = "Comedy",       Year = 2001, Duration = 122 },
            new() { Title = "A Clockwork Orange",                                   Genre = "Sci-Fi",       Year = 1971, Duration = 136 },
            new() { Title = "Full Metal Jacket",                                    Genre = "Drama",        Year = 1987, Duration = 116 },
            new() { Title = "Double Indemnity",                                     Genre = "Crime",        Year = 1944, Duration = 107 },
            new() { Title = "The Kid",                                              Genre = "Comedy",       Year = 1921, Duration = 68  }
        };

        await context.Movies.AddRangeAsync(movies);
        await context.SaveChangesAsync();
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Data;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly MovieContext _context;

    public ActorsController(MovieContext context)
    {
        _context = context;
    }

    // GET: api/actors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetActor()
    {
        var actors = await _context.Actors
            .Select(actor => new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.BirthYear
            })
            .ToListAsync();

        return Ok(actors);
    }      
    
    // GET: api/actors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetActor(int id)
    {
        var actorDto = await _context.Actors
            .Where(actor => actor.Id == id)
            .Select(actor => new ActorDto
            {
                Id = actor.Id,
                Name = actor.Name,
                BirthYear = actor.BirthYear
            })
            .FirstOrDefaultAsync();

        if (actorDto == null)
        {
            return NotFound();
        }

        return Ok(actorDto);
    }

    // POST: api/actors
    [HttpPost]
    public async Task<ActionResult<ActorDto>> CreateActor([FromBody] ActorCreateDto actorCreateDto)
    {
        var actor = new Actor
        {
            Name = actorCreateDto.Name,
            BirthYear = actorCreateDto.BirthYear
        };

        _context.Actors.Add(actor);
        await _context.SaveChangesAsync();

        var actorDto = new ActorDto
        {
            Id = actor.Id,
            Name = actor.Name,
            BirthYear = actor.BirthYear
        };

        return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actorDto);
    }

    // PUT: api/actors/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActor(int id, [FromBody] ActorCreateDto actorCreateDto)
    {
        var actor = await _context.Actors.FindAsync(id);
        if (actor == null)
        {
            return NotFound();
        }

        actor.Name = actorCreateDto.Name;
        actor.BirthYear = actorCreateDto.BirthYear;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}

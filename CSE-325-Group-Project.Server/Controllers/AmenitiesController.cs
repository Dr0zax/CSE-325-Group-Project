using CSE325Project.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSE325project.Shared;

[ApiController]
[Route("api/[controller]")]
public class AmenitiesController : ControllerBase
{
    private readonly StudySpotContext _context;

    public AmenitiesController(StudySpotContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Amenity>> Get()
    {
        return await _context.Amenity.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Amenity>> Get(long id)
    {
        var amenity = await _context.Amenity.FindAsync(id);
        if (amenity == null)
        {
            return NotFound();
        }
        return amenity;
    }

    [HttpPost]
    public async Task<ActionResult<Amenity>> Post(Amenity amenity)
    {
        _context.Amenity.Add(amenity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = amenity.AmenityId }, amenity);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Amenity>> Put(long id, Amenity amenity)
    {
        if (id != amenity.AmenityId)
        {
            return BadRequest();
        }

        _context.Entry(amenity).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Amenity>> Delete(long id)
    {
        var amenity = await _context.Amenity.FindAsync(id);
        if (amenity == null)
        {
            return NotFound();
        }

        _context.Amenity.Remove(amenity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
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
    public async Task<IEnumerable<Amenities>> Get()
    {
        return await _context.Amenities.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Amenities>> Get(long id)
    {
        var amenities = await _context.Amenities.FindAsync(id);
        if (amenities == null)
        {
            return NotFound();
        }
        return amenities;
    }

    [HttpPost]
    public async Task<ActionResult<Amenities>> Post(Amenities amenities)
    {
        _context.Amenities.Add(amenities);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = amenities.AmenityId }, amenities);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Amenities>> Put(long id, Amenities amenities)
    {
        if (id != amenities.AmenityId)
        {
            return BadRequest();
        }

        _context.Entry(amenities).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Amenities>> Delete(long id)
    {
        var amenities = await _context.Amenities.FindAsync(id);
        if (amenities == null)
        {
            return NotFound();
        }

        _context.Amenities.Remove(amenities);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
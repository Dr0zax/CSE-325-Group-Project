using CSE325Project.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSE325project.Shared;

[ApiController]
[Route("api/[controller]")]
public class RoomAmenitiesController : ControllerBase
{
    private readonly StudySpotContext _context;

    public RoomAmenitiesController(StudySpotContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<RoomAmenities>> Get()
    {
        return await _context.RoomAmenities.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomAmenities>> Get(long id)
    {
        var roomAmenities = await _context.RoomAmenities.FindAsync(id);
        if (roomAmenities == null)
        {
            return NotFound();
        }
        return roomAmenities;
    }

    [HttpPost]
    public async Task<ActionResult<RoomAmenities>> Post(RoomAmenities roomAmenities)
    {
        _context.RoomAmenities.Add(roomAmenities);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = roomAmenities.RoomAmenityId }, roomAmenities);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RoomAmenities>> Put(long id, RoomAmenities roomAmenities)
    {
        if (id != roomAmenities.RoomAmenityId)
        {
            return BadRequest();
        }

        _context.Entry(roomAmenities).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RoomAmenities>> Delete(long id)
    {
        var roomAmenities = await _context.RoomAmenities.FindAsync(id);
        if (roomAmenities == null)
        {
            return NotFound();
        }

        _context.RoomAmenities.Remove(roomAmenities);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
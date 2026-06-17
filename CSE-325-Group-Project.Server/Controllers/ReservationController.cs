using CSE325Project.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSE325project.Shared;

[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly StudySpotContext _context;

    public ReservationController(StudySpotContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Reservation>> Get()
    {
        return await _context.Reservations.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reservation>> Get(long id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return reservation;
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> Post(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = reservation.ReservationId }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Reservation>> Put(long id, Reservation reservation)
    {
        if (id != reservation.ReservationId)
        {
            return BadRequest();
        }

        _context.Entry(reservation).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Reservation>> Delete(long id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }

        _context.Reservations.Remove(reservation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
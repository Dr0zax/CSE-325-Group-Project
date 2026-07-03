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
    public async Task<ActionResult<Reservation>> Get(Guid id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return NotFound();
        }
        return reservation;
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var count = await _context.Reservations.CountAsync();
        return count;
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> Post(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = reservation.ReservationId }, reservation);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Reservation>> Put(Guid id, Reservation reservation)
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
    public async Task<ActionResult<Reservation>> Delete(Guid id)
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

    [HttpGet("counts")]
    public async Task<List<RoomReservationCountDto>> GetReservationCountsAsync()
    {
        return await _context.Reservations
            .GroupBy(r => r.RoomId)
            .Select(g => new RoomReservationCountDto
            {
                RoomId = g.Key,
                ReservationCount = g.Count()
            })
            .Join(_context.Rooms,
                g => g.RoomId,
                r => r.RoomId,
                (g, r) => new RoomReservationCountDto
                {
                    RoomId = r.RoomId,
                    RoomName = r.RoomName,
                    ReservationCount = g.ReservationCount
                })
            .ToListAsync();
    }

    [HttpGet("upcoming")]
    public async Task<List<UpcomingReservationsDto>> GetUpcomingReservationsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Room)
            .Where(r => r.StartTime > now)
            .Select(r => new UpcomingReservationsDto
            {
                ReservationId = r.ReservationId,
                Email = r.User.Email ?? "",
                RoomName = r.Room.RoomName ?? "",
                ReservationDate = r.StartTime,
                ReservationTime = r.StartTime.TimeOfDay
            })
            .ToListAsync();
    }

    [HttpGet ("reseravtions/manager")]

    public async Task<List<ReservationManagerDto>> GetReservationsForManagementAsync()
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Room)
            .Select(r => new ReservationManagerDto
            {
                ReservationId = r.ReservationId,
                Email = r.User.Email ?? "",
                RoomName = r.Room.RoomName ?? "",
                StartDate = r.StartTime,
                EndDate = r.EndTime,
                Status = r.Status ?? ""
            })
            .ToListAsync();
    }
}
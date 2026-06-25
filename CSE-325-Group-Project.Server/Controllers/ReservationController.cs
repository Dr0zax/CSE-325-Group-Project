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

    [HttpGet("slots")]
    public async Task<ActionResult<List<AppointmentSlot>>> GetSlots(long roomId, DateTime? weekStart)
    {
        var pickedDate = weekStart?.Date ?? DateTime.UtcNow.Date;
        var firstDay = DateTime.SpecifyKind(StartOfWeek(pickedDate), DateTimeKind.Utc);
        var lastDay = firstDay.AddDays(5);

        var reservations = await _context.Reservations
            .Where(reservation =>
                reservation.RoomId == roomId &&
                reservation.StartTime < lastDay.AddHours(17) &&
                reservation.EndTime > firstDay.AddHours(10) &&
                (reservation.Status == null || reservation.Status != "Canceled"))
            .ToListAsync();

        var slots = new List<AppointmentSlot>();

        for (var day = 0; day < 5; day++)
        {
            var date = firstDay.AddDays(day);

            for (var hour = 10; hour < 17; hour++)
            {
                var start = date.AddHours(hour);
                var end = start.AddHours(1);

                var reservation = reservations.FirstOrDefault(item =>
                    item.StartTime < end && item.EndTime > start);

                slots.Add(new AppointmentSlot
                {
                    RoomId = roomId,
                    StartTime = start,
                    EndTime = end,
                    IsAvailable = reservation == null,
                    ReservationId = reservation?.ReservationId
                });
            }
        }

        return slots;
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

    [HttpPost]
    public async Task<ActionResult<Reservation>> Post(Reservation reservation)
    {
        var start = DateTime.SpecifyKind(reservation.StartTime, DateTimeKind.Utc);
        var end = DateTime.SpecifyKind(reservation.EndTime, DateTimeKind.Utc);

        if (end <= start)
        {
            return BadRequest("Pick a valid time.");
        }

        var taken = await _context.Reservations.AnyAsync(item =>
            item.RoomId == reservation.RoomId &&
            item.StartTime < end &&
            item.EndTime > start &&
            (item.Status == null || item.Status != "Canceled"));

        if (taken)
        {
            return Conflict("That time is already reserved.");
        }

        if (reservation.UserId == Guid.Empty)
        {
            var user = await _context.Users.FirstOrDefaultAsync();
            if (user == null)
            {
                return BadRequest("No user found for the reservation.");
            }

            reservation.UserId = user.UserId;
        }

        var now = DateTime.UtcNow;
        reservation.StartTime = start;
        reservation.EndTime = end;
        reservation.CreatedAt = now;
        reservation.UpdatedAt = now;
        reservation.Status = string.IsNullOrWhiteSpace(reservation.Status)
            ? "Reserved"
            : reservation.Status;

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




    private static DateTime StartOfWeek(DateTime date)
    {
        var daysSinceMonday = ((int)date.DayOfWeek + 6) % 7;
        return date.AddDays(-daysSinceMonday);
    }
}

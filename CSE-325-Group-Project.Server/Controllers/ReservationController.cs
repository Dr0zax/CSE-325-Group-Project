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
}
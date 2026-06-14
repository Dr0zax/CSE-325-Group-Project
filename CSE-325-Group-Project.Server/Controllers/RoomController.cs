using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly StudyRoomContext _context;

    public RoomsController(StudyRoomContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Room>> Get()
    {
        return await _context.Rooms.ToListAsync();
    }
}
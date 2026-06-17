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
}
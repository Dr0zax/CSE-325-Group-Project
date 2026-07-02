using CSE325Project.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSE325project.Shared;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly StudySpotContext _context;

    public UsersController(StudySpotContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return await _context.Users.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> GetCount()
    {
        var count = await _context.Users.CountAsync();
        return count;
    }

    [HttpPost]
    public async Task<ActionResult<User>> Post(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = user.UserId }, user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Put(Guid id, User user)
    {
        if (id != user.UserId)
        {
            return BadRequest();
        }

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<User>> Delete(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    
}
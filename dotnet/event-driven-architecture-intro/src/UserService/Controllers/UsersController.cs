using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;

namespace UserService.Controllers;

[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserServiceContext _context;

    public UsersController(UserServiceContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers() => await _context.Users.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<User?>> GetUser(int id) => await _context.Users.FindAsync(id);

    [HttpPost]
    public async Task<ActionResult<User>> PostUser([FromBody]User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id },user);
    }

    [HttpPut]
    public async Task<ActionResult> PutUser([FromBody]User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Entities;
using UserService.Integration;

namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private const string ExchangeName = "user";
    private const string PostUserIntegrationEvent = "user.add";
    private const string PutUserIntegrationEvent = "user.update";
    
    private readonly UserServiceContext _context;
    private readonly IPublisher _publisher;

    public UsersController(UserServiceContext context,IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
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

        _publisher.PublishToMessageQueue(ExchangeName, PostUserIntegrationEvent, new
        {
            id = user.Id,
            name = user.Name
        });
        return CreatedAtAction(nameof(GetUser), new { id = user.Id },user);
    }

    [HttpPut]
    public async Task<ActionResult> PutUser([FromBody]User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        
        _publisher.PublishToMessageQueue(ExchangeName, PutUserIntegrationEvent, new
        {
            id = user.Id,
            newName = user.Name
        });
        return NoContent();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostService.Data;
using PostService.Entities;

namespace PostService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly PostServiceContext _context;

    public PostController(PostServiceContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts() =>
        await _context.Posts.Include(post => post.User).ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Post?>> GetPost(int id) =>
        await _context.Posts.Where(post => post.PostId == id)
            .Include(post => post.User).FirstOrDefaultAsync();


    [HttpPost]
    public async Task<ActionResult<Post>> PostPost([FromBody] Post post)
    {
        await _context.AddAsync(post);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = post.PostId, post });
    }

}
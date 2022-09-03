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
    public async Task<ActionResult<Post>> PostPost([FromBody] NewPost post)
    {
        var result = await _context.AddAsync(new Post
        {
            UserId = post.UserId,
            Content = post.Content,
            Title = post.Title
        });
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPost), new { id = result.Entity.PostId },post);
    }
}

public class NewPost
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int UserId { get; set; }
}
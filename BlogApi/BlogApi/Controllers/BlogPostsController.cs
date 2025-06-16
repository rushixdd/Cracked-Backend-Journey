using BlogDomain.Entities;
using BlogInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("posts")]
public class BlogPostsController : ControllerBase
{
    private readonly BlogDbContext _context;
    public BlogPostsController(BlogDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] BlogPost post)
    {
        if (string.IsNullOrWhiteSpace(post.Title) || string.IsNullOrWhiteSpace(post.Content))
            return BadRequest("Title and content are required.");

        post.CreatedAt = DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;

        _context.BlogPosts.Add(post);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(int id)
    {
        var post = await _context.BlogPosts.FindAsync(id);
        return post is null ? NotFound() : Ok(post);
    }
}
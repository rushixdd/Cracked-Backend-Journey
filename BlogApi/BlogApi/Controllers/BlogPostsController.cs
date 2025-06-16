using BlogApp.DTOs;
using BlogApp.Interfaces;
using BlogDomain.Entities;
using BlogInfrastructure.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("posts")]
public class BlogPostsController : ControllerBase
{
    private readonly IBlogPostService _blogService;

    public BlogPostsController(IBlogPostService blogService)
    {
        _blogService = blogService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
    {
        var created = await _blogService.CreatePostAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var post = await _blogService.GetPostByIdAsync(id);
        return post == null ? NotFound() : Ok(post);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? term)
    {
        var posts = await _blogService.GetAllPostsAsync(term);
        return Ok(posts);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePostDto dto)
    {
        var updated = await _blogService.UpdatePostAsync(id, dto);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _blogService.DeletePostAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
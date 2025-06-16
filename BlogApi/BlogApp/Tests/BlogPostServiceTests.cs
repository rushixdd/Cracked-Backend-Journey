using BlogApp.DTOs;
using BlogApp.Services;
using BlogInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BlogApp.Tests;
public class BlogPostServiceTests
{
    private readonly BlogDbContext _context;
    private readonly BlogPostService _service;

    public BlogPostServiceTests()
    {
        var options = new DbContextOptionsBuilder<BlogDbContext>()
            .UseInMemoryDatabase(databaseName: "BlogTestDb")
            .Options;

        _context = new BlogDbContext(options);
        _service = new BlogPostService(_context);
    }

    [Fact]
    public async Task CreatePost_ShouldReturnCreatedPost()
    {
        var dto = new CreatePostDto
        {
            Title = "Test Post",
            Content = "Some content",
            Category = "General",
            Tags = new List<string> { "Test", "XUnit" }
        };

        var result = await _service.CreatePostAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Test Post", result.Title);
        Assert.Equal("General", result.Category);
        Assert.True(result.Id > 0);
    }
}

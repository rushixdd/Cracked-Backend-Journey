using BlogApp.DTOs;
using BlogApp.Services;
using BlogInfrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace BlogApp.Tests
{
    public class BlogPostServiceTests
    {
        private readonly BlogDbContext _dbContext;
        private readonly BlogPostService _service;
        private readonly ILogger<BlogPostService> _logger;

        public BlogPostServiceTests()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: "BlogDb_Test")
                .Options;

            _dbContext = new BlogDbContext(options);

            // Add mock logger
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<BlogPostService>();

            _service = new BlogPostService(_dbContext, _logger);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_BlogPost()
        {
            var dto = new CreatePostDto
            {
                Title = "Test Post",
                Content = "Content of test post",
                Category = "UnitTest",
                Tags = new List<string> { "test", "blog" }
            };

            var result = await _service.CreatePostAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Title, result.Title);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Posts()
        {
            await _service.CreatePostAsync(new CreatePostDto
            {
                Title = "Post A",
                Content = "Content A",
                Category = "CategoryA",
                Tags = new List<string> { "A" }
            });

            var posts = await _service.GetAllPostsAsync("");

            Assert.NotNull(posts);
            Assert.True(posts.Any());
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Post()
        {
            var created = await _service.CreatePostAsync(new CreatePostDto
            {
                Title = "Single Post",
                Content = "Only One",
                Category = "Single",
                Tags = new List<string> { "One" }
            });

            var post = await _service.GetPostByIdAsync(created.Id);

            Assert.NotNull(post);
            Assert.Equal("Single Post", post.Title);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Post()
        {
            var created = await _service.CreatePostAsync(new CreatePostDto
            {
                Title = "Original",
                Content = "Original Content",
                Category = "Old",
                Tags = new List<string> { "old" }
            });

            var updatedDto = new UpdatePostDto
            {
                Title = "Updated",
                Content = "New Content",
                Category = "New",
                Tags = new List<string> { "new" }
            };

            var updated = await _service.UpdatePostAsync(created.Id, updatedDto);

            Assert.Equal("Updated", updated.Title);
            Assert.Equal("New", updated.Category);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Post()
        {
            var created = await _service.CreatePostAsync(new CreatePostDto
            {
                Title = "Delete Me",
                Content = "Will be gone",
                Category = "Trash",
                Tags = new List<string> { "delete" }
            });

            var deleted = await _service.DeletePostAsync(created.Id);
            var shouldBeNull = await _service.GetPostByIdAsync(created.Id);

            Assert.True(deleted);
            Assert.Null(shouldBeNull);
        }

        [Fact]
        public async Task SearchAsync_Should_Filter_By_Term()
        {
            await _service.CreatePostAsync(new CreatePostDto
            {
                Title = "C# is great",
                Content = "Programming language",
                Category = "Tech",
                Tags = new List<string> { "dotnet" }
            });

            var results = await _service.GetAllPostsAsync("c#");

            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.Title.Contains("C#"));
        }
    }
}

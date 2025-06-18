using BlogApp.DTOs;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BlogApi.IntegrationTests;

public class BlogPostControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public BlogPostControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        AuthenticateAsync().GetAwaiter().GetResult(); // authenticate once for all tests
    }

    private async Task AuthenticateAsync()
    {
        var request = new AuthRequest
        {
            Username = "postuser",
            Password = "secure123"
        };

        // Register
        await _client.PostAsJsonAsync("/auth/register", request);

        // Login
        var response = await _client.PostAsJsonAsync("/auth/login", request);
        response.EnsureSuccessStatusCode();
        var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
        // Add bearer token to client
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authResult!.Token);
    }

    [Fact]
    public async Task CreateBlogPost_Returns201Created()
    {
        var postRequest = new CreatePostDto
        {
            Title = "Integration Test Post",
            Content = "This is a test post.",
            Category = "Testing",
            Tags = new List<string> { "Integration", "Test" }
        };

        var response = await _client.PostAsJsonAsync("/posts", postRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var responseData = await response.Content.ReadFromJsonAsync<CreatePostDto>();
        responseData.Should().NotBeNull();
        responseData!.Title.Should().Be("Integration Test Post");
    }


    [Fact]
    public async Task GetAllBlogPosts_ReturnsPosts()
    {
        var createDto = new CreatePostDto
        {
            Title = "Seed Post",
            Content = "Seed content",
            Category = "General",
            Tags = new List<string> { "Seed" }
        };

        await _client.PostAsJsonAsync("/posts", createDto);

        // Act
        var response = await _client.GetAsync("/posts");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var posts = await response.Content.ReadFromJsonAsync<List<PostResponseDto>>();
        posts.Should().NotBeNull();
        posts!.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetBlogPostById_ReturnsCorrectPost()
    {
        var postRequest = new CreatePostDto
        {
            Title = "Single Post",
            Content = "Details for one post.",
            Category = "Testing",
            Tags = new List<string> { "One", "Test" }
        };

        var createResponse = await _client.PostAsJsonAsync("/posts", postRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<PostResponseDto>();
        created.Should().NotBeNull();

        var getResponse = await _client.GetAsync($"/posts/{created!.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var post = await getResponse.Content.ReadFromJsonAsync<CreatePostDto>();
        post!.Title.Should().Be("Single Post");
    }

    [Fact]
    public async Task UpdateBlogPost_ReturnsUpdatedPost()
    {
        var createDto = new CreatePostDto
        {
            Title = "Old Title",
            Content = "Old content",
            Category = "Dev",
            Tags = new List<string> { "Old" }
        };

        var createResponse = await _client.PostAsJsonAsync("/posts", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<PostResponseDto>();

        var updateDto = new UpdatePostDto
        {
            Title = "Updated Title",
            Content = "Updated content",
            Category = "Updated Dev",
            Tags = new List<string> { "Updated" }
        };

        var updateResponse = await _client.PutAsJsonAsync($"/posts/{created!.Id}", updateDto);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await updateResponse.Content.ReadFromJsonAsync<UpdatePostDto>();
        updated!.Title.Should().Be("Updated Title");
    }

    [Fact]
    public async Task DeleteBlogPost_Returns204NoContent()
    {
        var createDto = new PostResponseDto
        {
            Title = "To Delete",
            Content = "Delete this",
            Category = "Trash",
            Tags = new List<string> { "Delete" }
        };

        var createResponse = await _client.PostAsJsonAsync("/posts", createDto);
        var created = await createResponse.Content.ReadFromJsonAsync<PostResponseDto>();

        var deleteResponse = await _client.DeleteAsync($"/posts/{created!.Id}");
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getResponse = await _client.GetAsync($"/posts/{created.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task SearchPosts_ReturnsMatchingResults()
    {
        var createDto = new PostResponseDto
        {
            Title = "Search Post",
            Content = "This post should appear in search",
            Category = "Search",
            Tags = new List<string> { "FindMe" }
        };

        await _client.PostAsJsonAsync("/posts", createDto);

        var response = await _client.GetAsync("/posts?term=search");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var posts = await response.Content.ReadFromJsonAsync<List<CreatePostDto>>();
        posts.Should().NotBeNull();
        posts!.Any(p => p.Title.Contains("Search", StringComparison.OrdinalIgnoreCase)).Should().BeTrue();
    }
}

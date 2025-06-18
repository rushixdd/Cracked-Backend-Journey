using System.Net;
using System.Net.Http.Json;
using BlogApp.DTOs;
using FluentAssertions;

namespace BlogApi.IntegrationTests;

public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AuthControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ShouldSucceed_WhenUsernameIsUnique()
    {
        var request = new AuthRequest
        {
            Username = "testuser1",
            Password = "password123"
        };

        var response = await _client.PostAsJsonAsync("/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result.Should().NotBeNull();
        result!.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public async Task Register_ShouldFail_WhenUsernameExists()
    {
        var request = new AuthRequest { Username = "existing", Password = "pass" };

        // Register first time
        await _client.PostAsJsonAsync("/auth/register", request);

        // Register second time (should fail)
        var response = await _client.PostAsJsonAsync("/auth/register", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json!["message"].Should().Be("Username already exists.");
    }

    [Fact]
    public async Task Login_ShouldSucceed_WithValidCredentials()
    {
        var request = new AuthRequest { Username = "loginuser", Password = "pass123" };

        // Register first
        await _client.PostAsJsonAsync("/auth/register", request);

        // Login
        var response = await _client.PostAsJsonAsync("/auth/login", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        result!.IsAuthenticated.Should().BeTrue();
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_ShouldFail_WithWrongPassword()
    {
        var request = new AuthRequest { Username = "wrongpassuser", Password = "correct" };

        await _client.PostAsJsonAsync("/auth/register", request);

        var loginAttempt = new AuthRequest { Username = "wrongpassuser", Password = "incorrect" };
        var response = await _client.PostAsJsonAsync("/auth/login", loginAttempt);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json!["message"].Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Login_ShouldFail_WhenUserDoesNotExist()
    {
        var response = await _client.PostAsJsonAsync("/auth/login", new AuthRequest
        {
            Username = "nouser",
            Password = "nopass"
        });

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        json!["message"].Should().Be("Invalid credentials.");
    }
}
